import { useState } from "react";

function Account() {
  const [accounts, setAccounts] = useState([]);
  const [message, setMessage] = useState("");

  const token = localStorage.getItem("token");

const createAccount = async () => {
  console.log("Create Account button clicked");

  if (!token) {
    console.log("No token found");
    setMessage("Please login first");
    return;
  }

  try {
    console.log("Sending POST request to AccountService");

    const response = await fetch(
      "http://localhost:5028/api/accounts/create",
      {
        method: "POST",
        headers: {
          "Authorization": "Bearer " + token,
          "Content-Type": "application/json"
        }
      }
    );

    console.log("Response status:", response.status);

    if (!response.ok) {
      setMessage("Failed to create account");
      return;
    }

    setMessage("Account created successfully");
    loadMyAccounts();

  } catch (error) {
    console.error("Error during createAccount:", error);
    setMessage("Error while creating account");
  }
};


  const loadMyAccounts = async () => {
    if (!token) {
      setMessage("Please login first");
      return;
    }

    const response = await fetch(
      "http://localhost:5028/api/accounts/my",
      {
        headers: {
          "Authorization": "Bearer " + token
        }
      }
    );

    if (!response.ok) {
      setMessage("Failed to load accounts");
      return;
    }

    const data = await response.json();
    setAccounts(data);
  };

  return (
    <div>
      <h2>My Bank Accounts</h2>

      <button onClick={createAccount}>
        Create Account
      </button>

      <button onClick={loadMyAccounts} style={{ marginLeft: "10px" }}>
        Load My Accounts
      </button>

      <p>{message}</p>

      <ul>
        {accounts.map(acc => (
          <li key={acc.id}>
            Account #{acc.id} — Balance: ₹{acc.balance}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default Account;
