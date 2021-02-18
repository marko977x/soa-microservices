import * as signalR from "@microsoft/signalr";
import { apiFetch } from "./api-fetch";

Object.defineProperty(WebSocket, 'OPEN', { value: 1, });

export const connection = new signalR.HubConnectionBuilder()
  .withUrl(`http://localhost:5006/event`, { transport: 1 })
  .configureLogging(signalR.LogLevel.Trace)
  .withAutomaticReconnect()
  .build();

connection.start();

connection.onreconnected(() => {
  apiFetch('POST', `http://localhost:5006/api/Analytics/Subscribe/?connectionId=${connection.connectionId}`);
});