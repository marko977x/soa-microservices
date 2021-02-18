import { Card, CardContent, Typography } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { apiFetch } from "../services/api-fetch";
import { connection } from "../services/hub-connections";
import "./notification.css";

interface INotification {
  name: string,
  text: string,
  background: string
}

interface NotificationImagePair {
  [key: string]: string
}

const images: NotificationImagePair = {
  "Suncano": "sunny.png",
  "Oblacno": "cloudy.png"
}

function Notification() {
  const [notification, setNotification] = useState("Nema dogadjaja od vaznosti!");

  connection.on("SendEvent", data => {
    setNotification(data as string);
  });

  useEffect(() => {
    apiFetch('POST', `http://localhost:5006/api/Analytics/Subscribe/?connectionId=${connection.connectionId}`);
  }, [])

  return (
    <div className="notification">
      <Card>
        <CardContent>
          <Typography variant="body2" color="textSecondary" component="p">{notification}</Typography>
        </CardContent>
      </Card>
    </div >
  );
}

export default Notification;