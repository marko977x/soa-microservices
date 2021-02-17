import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@material-ui/core";
import React from "react";
import { IWeatherData } from "../App";
import "./weather-data.css";

function WeatherData(props: {data: IWeatherData}) {

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
            {props.data.sensors.map((parameter, rowIndex) =>
              <TableRow key={rowIndex}>
                <TableCell component="th" scope="row">{parameter.name}</TableCell>
                <TableCell align="center">{parameter.minValue?.toFixed(2)}</TableCell>
                <TableCell align="center">{parameter.maxValue?.toFixed(2)}</TableCell>
                <TableCell align="center">{parameter.latestValue?.toFixed(2)}</TableCell>
                <TableCell align="center">{parameter.lastMinuteAvgValue?.toFixed(2)}</TableCell>
                <TableCell align="center">{parameter.lastHourAvgValue?.toFixed(2)}</TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>
    </div>
  );
}

export default WeatherData;