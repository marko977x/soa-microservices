import React from 'react';
import './App.css';
import Notification from './components/Notification';
import WeatherData from './components/WeatherData';

function App() {
  return (
    <div className="app">
      <div className="content">
        <WeatherData></WeatherData>
        <Notification></Notification>
      </div>
    </div>
  );
}

export default App;
