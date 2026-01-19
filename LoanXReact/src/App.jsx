import React from 'react' 
import Login from './Pages/Login.jsx' 
import './App.css'
import Account from './Account.jsx' 
import Register from './Register.jsx'
import Loan from './Loan.jsx' 

function App() {

  return (
    <div>
      <h1>Welcome to LoanX</h1>

      <Register />
      <hr />

      <Login />
      <hr />

      <Account />
      <Loan />
    </div>
  )
}

export default App
