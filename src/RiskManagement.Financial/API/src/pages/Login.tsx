import { useMsal } from '@azure/msal-react'
import { loginRequest } from '../authConfig'

export default function Login() {
  const { instance } = useMsal()

  const handleLogin = () => {
    instance.loginRedirect(loginRequest).catch(console.error)
  }

  return (
    <div style={{
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center',
      height: '100vh',
      backgroundColor: '#f5f5f5'
    }}>
      <div style={{
        textAlign: 'center',
        padding: '2rem',
        backgroundColor: 'white',
        borderRadius: '8px',
        boxShadow: '0 2px 10px rgba(0,0,0,0.1)'
      }}>
        <h1>Risk Management System</h1>
        <p>Inicia sesión con tu cuenta corporativa</p>
        <button
          onClick={handleLogin}
          style={{
            padding: '10px 20px',
            backgroundColor: '#0078d4',
            color: 'white',
            border: 'none',
            borderRadius: '4px',
            cursor: 'pointer',
            fontSize: '16px'
          }}
        >
          Iniciar sesión con Microsoft
        </button>
      </div>
    </div>
  )
}
