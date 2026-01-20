// import { useState } from "react";

// function Loan() {
//   const [amount, setAmount] = useState("");
//   const [months, setMonths] = useState("");
//   const [loans, setLoans] = useState([]);
//   const [message, setMessage] = useState("");

//   const token = localStorage.getItem("token");

//   // const applyLoan = async () => {
//   //   try {
//   //     const response = await fetch(
//   //       "http://localhost:5272/api/loans/apply",
//   //       {
//   //         method: "POST",
//   //         headers: {
//   //           "Authorization": "Bearer " + token,
//   //           "Content-Type": "application/json"
//   //         },
//   //         body: JSON.stringify({
//   //           amount: Number(amount),
//   //           tenureMonths: Number(months)
//   //         })
//   //       }
//   //     );

//   //     const data = await response.json();
//   //     setMessage("Loan applied. EMI: ₹" + data.emi);
//   //     loadMyLoans();

//   //   } catch {
//   //     setMessage("Loan apply failed");
//   //   }
//   // };

//   const applyLoan = async () => {
//       try {
//         const token = localStorage.getItem("token");
//         if (!token) {
//           setMessage("Authentication token missing. Please log in.");
//           // Optionally redirect to login page
//           // window.location.href = "/login";
//           return;
//         }

//         const response = await fetch(
//           "http://localhost:5272/api/loans/apply",
//           {
//             method: "POST",
//             headers: {
//               "Authorization": "Bearer " + token,
//               "Content-Type": "application/json"
//             },
//             body: JSON.stringify({
//               amount: Number(amount),
//               tenureMonths: Number(months)
//             })
//           }
//         );

//         if (!response.ok) { // Check if the response status is not 2xx
//           if (response.status === 401) {
//             setMessage("Authentication failed. Please log in again.");
//             // Optionally clear token and redirect to login page
//             // localStorage.removeItem("token");
//             // window.location.href = "/login";
//           } else {
//             const errorData = await response.json(); // Try to parse error details
//             setMessage(`Loan apply failed: ${errorData.message || response.statusText}`);
//           }
//           return; // Stop execution if request was not successful
//         }

//         const data = await response.json();
//         setMessage("Loan applied successfully. Status: " + data.status + 
//         " | EMI: ₹" + data.emi);

//         loadMyLoans();

//       } catch (error) {
//         console.error("Error applying loan:", error);
//         setMessage("An unexpected error occurred. Loan apply failed.");
//       }
//     };

//   const loadMyLoans = async () => {
//     const response = await fetch(
//       "http://localhost:5272/api/loans/my",
//       {
//         headers: {
//           "Authorization": "Bearer " + token,
//           "Content-Type": "application/json"
//         }
//       }
//     );

//     const data = await response.json();
//     setLoans(data);
//   };

//   return (
//     <div>
//       <h2>Apply Loan</h2>

//       <input
//         placeholder="Amount"
//         value={amount}
//         onChange={e => setAmount(e.target.value)}
//       />

//       <input
//         placeholder="Tenure (months)"
//         value={months}
//         onChange={e => setMonths(e.target.value)}
//       />

//       <button onClick={applyLoan}>Apply</button>

//       <p>{message}</p>

//       <h3>My Loans</h3>
//       <ul>
//         {loans.map(l => (
//           <li key={l.id}>
//             ₹{l.amount} | EMI ₹{l.emi} | {l.status}
//           </li>
//         ))}
//       </ul>
//     </div>
//   );
// }

// export default Loan;

import { useEffect, useState } from "react";

function Loan() {
  const [amount, setAmount] = useState("");
  const [months, setMonths] = useState("");
  const [loans, setLoans] = useState([]);
  const [message, setMessage] = useState("");

  const token = localStorage.getItem("token");

  const applyLoan = async () => {
    const response = await fetch(
      "http://localhost:5272/api/loans/apply",
      {
        method: "POST",
        headers: {
          Authorization: "Bearer " + token,
          "Content-Type": "application/json"
        },
        body: JSON.stringify({
          amount: Number(amount),
          tenureMonths: Number(months)
        })
      }
    );

    const data = await response.json();
    setMessage(
      `Loan applied. Status: ${data.status}, EMI: ₹${data.emi}`
    );

    loadMyLoans();
  };

  const loadMyLoans = async () => {
    const response = await fetch(
      "http://localhost:5272/api/loans/my",
      {
        headers: {
          Authorization: "Bearer " + token
        }
      }
    );

    const data = await response.json();
    setLoans(data);
  };

  const payEmi = async (loanId) => {
    await fetch(
      `http://localhost:5272/api/loans/pay-emi/${loanId}`,
      {
        method: "POST",
        headers: {
          Authorization: "Bearer " + token
        }
      }
    );

    setMessage("EMI paid successfully");
    loadMyLoans();
  };

  useEffect(() => {
    loadMyLoans();
  }, []);

  return (
    <div>
      <h2>Loan Section</h2>

      <input
        placeholder="Loan Amount"
        value={amount}
        onChange={e => setAmount(e.target.value)}
      />

      <input
        placeholder="Tenure (months)"
        value={months}
        onChange={e => setMonths(e.target.value)}
      />

      <button onClick={applyLoan}>Apply Loan</button>

      <p>{message}</p>

      <h3>My Loans</h3>

      <ul>
        {loans.map(loan => (
          <li key={loan.id}>
            Amount: ₹{loan.amount} |
            EMI: ₹{loan.emi} |
            Status: {loan.status} |
            Outstanding: ₹{loan.outstandingAmount}

            {/* {loan.status === "Disbursed" && (
              <button
                style={{ marginLeft: "10px" }}
                onClick={() => payEmi(loan.id)}
              >
                Pay EMI ₹{loan.emi}
              </button>
            )} */}
{loan.status?.toString().toLowerCase() === "disbursed" && (
  <button
    style={{ marginLeft: "10px" }}
    onClick={() => payEmi(loan.id)}
  >
    Pay EMI ₹{loan.emi.toFixed(2)}
  </button>
)}


        {loans.map(loan => {
  console.log("LOAN OBJECT:", loan);
  return (
    <li key={loan.id}>
      Amount: ₹{loan.amount} | EMI: ₹{loan.emi} | Status: {loan.status}
    </li>
  );
})}



          </li>
        ))}
      </ul>
    </div>
  );
}

export default Loan;

