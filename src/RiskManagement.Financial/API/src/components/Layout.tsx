import { useMsal } from '@azure/msal-react'
import { Link, useLocation } from 'react-router-dom'

export default function Layout({ children }: { children: React.ReactNode }) {
  const { instance, accounts } = useMsal()
  const location = useLocation()

  const handleLogout = () => {
    instance.logoutRedirect()
  }

  const menuItems = [
    { path: '/', label: 'Dashboard', icon: ' ' },
    { path: '/questionnaire', label: 'Cuestionario', icon: ' ' },
    { path: '/risk-map', label: 'Mapa de Calor', icon: ' ' },
    { path: '/audit', label: 'Auditoría', icon: ' ' },
  ]

  return (
    <div style={{ display: 'flex', height: '100vh' }}>
      <nav style={{ width: '250px', backgroundColor: '#1a237e', color: 'white', padding: '1rem' }}>
        <h2 style={{ marginBottom: '2rem' }}>Risk Manager</h2>
        <ul style={{ listStyle: 'none', padding: 0, margin: 0 }}>
          {menuItems.map(item => (
            <li key={item.path} style={{ marginBottom: '0.5rem' }}>
              <Link
                to={item.path}
                style={{
                  display: 'block',
                  padding: '10px',
                  color: location.pathname === item.path ? '#fff' : '#b0bec5',
                  textDecoration: 'none',
                  backgroundColor: location.pathname === item.path ? 'rgba(255,255,255,0.1)' : 'transparent',
                  borderRadius: '4px'
                }}
              >
                {item.icon} {item.label}
              </Link>
            </li>
          ))}
        </ul>
      </nav>

      <div style={{ flex: 1, display: 'flex', flexDirection: 'column' }}>
        <header style={{ padding: '10px 20px', borderBottom: '1px solid #ddd', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <span>Hola, {accounts[0]?.name}</span>
          <button
            onClick={handleLogout}
            style={{ padding: '8px 16px', backgroundColor: '#f44336', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}
          >
            Cerrar sesión
          </button>
        </header>
        <main style={{ flex: 1, padding: '1rem', overflow: 'auto' }}>
          {children}
        </main>
      </div>
    </div>
  )
}
