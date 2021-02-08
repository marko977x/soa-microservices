import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@material-ui/core";
import React, { useEffect } from "react";
import { apiFetch } from "../services/api-fetch";
import { GET_MIN_VALUE_FROM_ALL_SENSORS } from "../services/endpoints";
import "./weather-data.css";

interface IWeatherData {
  parameters: IWeatherParameter[]
}

interface IWeatherParameter {
  name: string,
  minValue: number,
  maxValue: number,
  latestValue: number,
  lastMinuteAvgValue: number,
  lastHourAvgValue: number
}

const exampleWeatherData = {
  parameters: [
    {
      name: "Temperature",
      minValue: 6,
      maxValue: 14,
      latestValue: 8,
      lastMinuteAvgValue: 10.5,
      lastHourAvgValue: 9.9
    },
    {
      name: "Humidity",
      minValue: 5,
      maxValue: 19,
      latestValue: 13,
      lastMinuteAvgValue: 12,
      lastHourAvgValue: 13
    },
    {
      name: "Pressure",
      minValue: 7,
      maxValue: 15,
      latestValue: 14,
      lastMinuteAvgValue: 9.5,
      lastHourAvgValue: 10.0
    }
  ]
}

function WeatherData() {
  let weatherData: IWeatherData = exampleWeatherData;

  useEffect(() => {
    // updateMinValue();
    // updateMaxValue();

    // interval(5000).subscribe(() => { updateLatestValue() });
    // interval(10000).subscribe(() => { updateLastMinuteValues() });
    // interval(15000).subscribe(() => { updateLastHourValues() });
  }, []);

  function updateMinValue() {
    apiFetch("GET", GET_MIN_VALUE_FROM_ALL_SENSORS).then(response => {
      if (response.ok) return response.json();
      return null;
    }).then(result => {
      console.log(result);
    })
    console.log("min value updated");
  }

  function updateMaxValue() {
    console.log("max value updated");
  }

  function updateLatestValue() {
    console.log("latest value updated");
  }

  function updateLastMinuteValues() {
    console.log("last minute values updated");
  }

  function updateLastHourValues() {
    console.log("last hour values updated");
  }

  return (
    <div className="weather-data">
      <TableContainer>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Parameter name</TableCell>
              <TableCell>Min</TableCell>
              <TableCell>Max</TableCell>
              <TableCell>Latest</TableCell>
              <TableCell>Last minute</TableCell>
              <TableCell>Last hour</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {weatherData?.parameters?.map((parameter, rowIndex) =>
              <TableRow key={rowIndex}>
                <TableCell component="th" scope="row">{parameter.name}</TableCell>
                <TableCell align="center">{parameter.minValue}</TableCell>
                <TableCell align="center">{parameter.maxValue}</TableCell>
                <TableCell align="center">{parameter.latestValue}</TableCell>
                <TableCell align="center">{parameter.lastMinuteAvgValue}</TableCell>
                <TableCell align="center">{parameter.lastHourAvgValue}</TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>
    </div>
  );
}

export default WeatherData;