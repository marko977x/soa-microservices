import React, { useEffect, useState } from 'react';
import { interval, Subscription, timer } from 'rxjs';
import './App.css';
import Notification from './components/Notification';
import SensorSetup from './components/SensorSetup';
import WeatherData from './components/WeatherData';
import { apiFetch } from './services/api-fetch';
import { GATEWAY } from './services/endpoints';

export interface IWeatherData {
  sensors: IWeatherParameter[]
}

export interface IWeatherParameter {
  name: string,
  minValue: number,
  maxValue: number,
  latestValue: number,
  lastMinuteAvgValue: number,
  lastHourAvgValue: number,
  timeout: number | undefined,
  threshold: number | undefined,
  isThreshold: boolean
}

const exampleWeatherData = {
  sensors: [
    {
      name: "Temperature",
      minValue: 0,
      maxValue: 0,
      latestValue: 0,
      lastMinuteAvgValue: 0,
      lastHourAvgValue: 0,
      timeout: 3000,
      threshold: 0,
      isThreshold: false
    },
    {
      name: "Humidity",
      minValue: 0,
      maxValue: 0,
      latestValue: 0,
      lastMinuteAvgValue: 0,
      lastHourAvgValue: 0,
      timeout: 3000,
      threshold: 0,
      isThreshold: false
    },
    {
      name: "Pressure",
      minValue: 0,
      maxValue: 0,
      latestValue: 0,
      lastMinuteAvgValue: 0,
      lastHourAvgValue: 0,
      timeout: 3000,
      threshold: 0,
      isThreshold: false
    }
  ]
}

function App() {
  const [weatherData, setWeatherData] = useState(exampleWeatherData);
  const [subscriptions, setSubscriptions] = useState<Subscription[]>([]);


  useEffect(() => {
    const fetchData = async (index: number) => {
      updateLatestValue(index);
      updateMinValue(index);
      updateMaxValue(index);
      updateLastMinuteValues(index);
      updateLastHourValues(index);
    }

    updateSensorsParams();
    weatherData.sensors.forEach((sensor, index) => {
      if (sensor.timeout) {
        let newSubscriptions = subscriptions;
        newSubscriptions[index] = timer(1000, sensor.timeout).subscribe(() => {
          fetchData(index);
        });
        setSubscriptions(newSubscriptions);
      }
    });
  }, []);

  const changeSubscription = (type: string, timeout: number) => {
    let subscriptionIndex = weatherData.sensors.findIndex(sensor => sensor.name == type);
    if (subscriptionIndex == -1) return;

    let weather = weatherData;
    weather.sensors[weather.sensors.findIndex(sensor => sensor.name == type)].isThreshold = false;
    weather.sensors[weather.sensors.findIndex(sensor => sensor.name == type)].timeout = timeout;
    setWeatherData(weatherData);

    setSensorTimeout(type, timeout);
    subscriptions[subscriptionIndex].unsubscribe();
    
    let newSubscriptions = subscriptions;
    newSubscriptions[subscriptionIndex] = interval(timeout).subscribe(() => {
      updateLatestValue(subscriptionIndex);
      updateMinValue(subscriptionIndex);
      updateMaxValue(subscriptionIndex);
      updateLastMinuteValues(subscriptionIndex);
      updateLastHourValues(subscriptionIndex);
    });
    setSubscriptions(newSubscriptions);
  }

  const updateMinValue = (index: number) => {
    apiFetch('GET', `${GATEWAY}/api/Gateway/GetMinValue/${weatherData.sensors[index].name}`
      ).then(response => {
        return response.json();
      }).then(result => {
        if (result == null) return;
        let data = {...weatherData};
        data.sensors[index].minValue = result.values._value;
        setWeatherData(data);
      });
  }

  const updateMaxValue = (index: number) => {
    apiFetch('GET', `${GATEWAY}/api/Gateway/GetMaxValue/${weatherData.sensors[index].name}`
      ).then(response => {
        return response.json();
      }).then(result => {
        if (result == null) return;
        let data = {...weatherData};
        data.sensors[index].maxValue = result.values._value;
        setWeatherData(data);
      });
  }

  const updateLatestValue = (index: number) => {
    apiFetch('GET', `${GATEWAY}/api/Gateway/GetSensorCurrentValue/${weatherData.sensors[index].name}`
      ).then(response => {
        return response.json();
      }).then(result => {
        if (result == null) return;
        let data = {...weatherData};
        data.sensors[index].latestValue = result.values._value;
        setWeatherData(data);
      });
  }

  const updateLastMinuteValues = (index: number) => {
    apiFetch('GET', `${GATEWAY}/api/Gateway/GetLastNMinutesValues/${weatherData.sensors[index].name}/?minutes=1`
      ).then(response => {
        return response.json();
      }).then(result => {
        if (result == null) return;
        let data = {...weatherData};
        data.sensors[index].lastMinuteAvgValue = result.values._value;
        setWeatherData(data);
      });
  }

  const updateLastHourValues = (index: number) => {
    apiFetch('GET', `${GATEWAY}/api/Gateway/GetLastNHoursMeanValue/${weatherData.sensors[index].name}/?hours=1`
      ).then(response => {
        return response.json();
      }).then(result => {
        if (result == null) return;
        let data = {...weatherData};
        data.sensors[index].lastHourAvgValue = result.values._value;
        setWeatherData(data);
      });
  }

  const updateSensorsParams = () => {
    apiFetch('GET', `${GATEWAY}/api/Gateway/GetAllSensorsParams`)
      .then(response => response.json())
      .then(result => {
        if (result == null) return;
        let data = {...weatherData};
        result.forEach((item: any, index: number) => {
          data.sensors[index].isThreshold = item.isThreshold;
          data.sensors[index].threshold = item.threshold;
          data.sensors[index].timeout = item.timeout;
        });
        setWeatherData(data);
      });
  }

  const setSensorTimeout = (sensor: string, timeout: number) => {
    apiFetch('POST', `${GATEWAY}/api/Gateway/SetTimeout/${sensor}/?value=${timeout}`);
  }

  const setSensorThreshold = (sensor: string, threshold: number) => {
    apiFetch('POST', `${GATEWAY}/api/Gateway/SetThreshold/${sensor}/?value=${threshold}`);
  }

  const updateThreshold = (type: string, threshold: number) => {
    if (threshold == null || threshold.toString() == "NaN") return;
    
    setSensorThreshold(type, threshold);
    let weather = weatherData;
    weather.sensors[weather.sensors.findIndex(sensor => sensor.name == type)].isThreshold = true;
    weather.sensors[weather.sensors.findIndex(sensor => sensor.name == type)].threshold = threshold;
    setWeatherData(weatherData);
  }
  
  return (
    <div className="app">
      <div className="content">
        <WeatherData data={weatherData}></WeatherData>
        <Notification></Notification>
        <SensorSetup
          weatherData={weatherData}
          onThresholdSave={(sensor, threshold) => updateThreshold(sensor, threshold)}
          onTimeoutSave={(sensor, timeout) => changeSubscription(sensor, timeout)}
        ></SensorSetup>
      </div>  
    </div>
  );
}

export default App;
