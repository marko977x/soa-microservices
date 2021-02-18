import { Paper, Typography } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { apiFetch } from "../services/api-fetch";
import { connection } from "../services/hub-connections";
import "./notification.css";

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
      <Paper className="paper">
        <Typography className="text" variant="body2" color="primary" component="p">{notification}</Typography>
      </Paper>
    </div >
  );
}

export default Notification;