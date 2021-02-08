import * as signalR from "@microsoft/signalr";

Object.defineProperty(WebSocket, 'OPEN', { value: 1, });

export const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:5001/", { transport: 1 })
  .configureLogging(signalR.LogLevel.Trace)
  .withAutomaticReconnect()
  .build();

// connection.start();