import { Card, CardContent, CardMedia, Typography } from "@material-ui/core";
import React from "react";
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
  let notification: INotification = {
    name: "Suncano",
    text: "Danas ce biti suncano vreme.",
    background: "sunny.png"
  };

  // connection.on("SendNotification", data => {
  //   console.log("Client received notification: " + data);
  // });

  return (
    <div className="notification">
      <Card>
        <CardMedia
          component="img"
          image={notification.background}
          title={notification.name}
        ></CardMedia>
        <CardContent>
          <Typography variant="body2" color="textSecondary" component="p">{notification.text}</Typography>
        </CardContent>
      </Card>
    </div >
  );
}

export default Notification;