import { useState } from "react";

function SecurePage() {
  const [message, setMessage] = useState("");

  const callSecureApi = async () => {
    const token = localStorage.getItem("token");

    if (!token) {
      setMessage("No token found. Please login first.");
      return;
    }

    try {
      const response = await fetch("http://localhost:5029/api/test/secure", {
        method: "GET",
        headers: {
          "Authorization": "Bearer " + token
        }
      });

      if (!response.ok) {
        setMessage("Access denied");
        return;
      }

      const text = await response.text();
      setMessage(text);
    } catch (error) {
      console.error(error);
      setMessage("Error calling secure API");
    }
  };

  return (
    <div>
      <h2>Secure Page</h2>
      <button onClick={callSecureApi}>
        Call Secure API
      </button>
      <p>{message}</p>
    </div>
  );
}

export default SecurePage;
