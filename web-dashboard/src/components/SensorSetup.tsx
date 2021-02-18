import { Button, FormControl, FormControlLabel, NativeSelect, Radio, RadioGroup, TextField, Typography } from "@material-ui/core";
import React, { ChangeEvent, useEffect, useState } from "react";
import { IWeatherData } from "../App";
import "./sensor-setup.css";

interface ISensorSetupProps {
  weatherData: IWeatherData,
  onTimeoutSave: (sensor: string, timeout: number) => void,
  onThresholdSave: (sensor: string, threshold: number) => void,
}

const THRESHOLD: string = "Threshold";
const TIMEOUT: string = "Timeout";

function SensorSetup(props: ISensorSetupProps) {
  const [selectedSensor, setSelectedSensor] = useState("Temperature");
  const [sensor, setSensor] = useState(props.weatherData.sensors.find(sensor => sensor.name == selectedSensor));
  const [selectedValue, setselectedValue] = useState(sensor?.isThreshold ? sensor.threshold : sensor?.timeout);
  const [option, setOption] = useState(sensor && sensor?.isThreshold ? THRESHOLD : TIMEOUT);
  const [initialUpdate, setInitialUpdate] = useState(2);

  useEffect(() => {
    if (initialUpdate > 0) {
      let mysensor = props.weatherData.sensors.find(item => item.name == selectedSensor);
      console.log(mysensor);
      if (option == THRESHOLD && mysensor && mysensor.threshold != selectedValue) {
        setselectedValue(mysensor.threshold);
      }
      else if (option == TIMEOUT && mysensor && mysensor.timeout != selectedValue) {
        setselectedValue(mysensor.timeout);
      }
      setInitialUpdate(initialUpdate - 1);
    }
  }, [props.weatherData])
  
  const handleSensorChange = (event: ChangeEvent<HTMLSelectElement>) => {
    setSelectedSensor(event.target.value);
    let newSensor = props.weatherData.sensors.find(item => item.name == event.target.value);
    setSensor(newSensor);
    setOption(newSensor && newSensor?.isThreshold ? THRESHOLD : TIMEOUT);
    setselectedValue(newSensor?.isThreshold ? newSensor.threshold : newSensor?.timeout);
  }

  const handleModeChange = (event: ChangeEvent<HTMLInputElement>) => {
    event.target.value == THRESHOLD ? setselectedValue(sensor?.threshold) : setselectedValue(sensor?.timeout);
    setOption(event.target.value);
  }

  const handleTimeoutThresholdChange = (event: ChangeEvent<HTMLInputElement>) => {
    setselectedValue(parseFloat(event.target.value));
  }

  const handleApplyClick = () => {
    if (option == THRESHOLD)
      props.onThresholdSave(selectedSensor, selectedValue as number);
    else
      props.onTimeoutSave(selectedSensor, selectedValue as number);
  }

  return (
    <div className="sensor-setup">
      <FormControl variant="filled">
        <NativeSelect
          value={selectedSensor}
          onChange={handleSensorChange}>
          {props.weatherData.sensors.map((sensor, index) => {
            return (<option key={index} value={sensor.name}>{sensor.name} sensor</option>)
          })}
        </NativeSelect>
      </FormControl>
      <Typography color="secondary">Currently active: {sensor?.isThreshold ? "Threshold" : "Timeout"}</Typography>
      <FormControl component="fieldset">
        <RadioGroup value={option} onChange={handleModeChange}>
          <div className="radio-buttons">
            <FormControlLabel value={TIMEOUT} control={<Radio />} label="Timeout" />
            <FormControlLabel value={THRESHOLD} control={<Radio />} label="Threshold" />
          </div>
        </RadioGroup>
      </FormControl>
      <TextField  type="number" value={selectedValue}
        onChange={handleTimeoutThresholdChange}></TextField>
      <Button color="primary" variant="contained" onClick={handleApplyClick}>Apply</Button>
    </div>
  );
}

export default SensorSetup;