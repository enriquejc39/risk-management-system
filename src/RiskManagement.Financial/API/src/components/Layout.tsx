import { useMsal } from '@azure/msal-react'
import { Link, useLocation } from 'react-router-dom'

export default function Layout({ children }: { children: React.ReactNode }) {
  const { instance, accounts } = useMsal()
  const location = useLocation()

  const handleLogout = () => {
    instance.logoutRedirect()
  }

  const menuSections = [
    {
      label: 'Dashboard',
      items: [
        { path: '/', label: 'Dashboard Ejecutivo', icon: '📊' },
        { path: '/risk-catalog', label: 'Catálogo de Riesgos', icon: '📋' },
        { path: '/risk-map', label: 'Mapa de Calor', icon: '🔥' },
      ]
    },
    {
      label: 'Gestión',
      items: [
        { path: '/questionnaire', label: 'Identificar Riesgo', icon: '📝' },
        { path: '/controls', label: 'Biblioteca de Controles', icon: '🛡️' },
        { path: '/action-plans', label: 'Planes de Acción', icon: '✅' },
      ]
    },
    {
      label: 'Auditoría',
      items: [
        { path: '/audit', label: 'Auditoría ISO', icon: '🔍' },
        { path: '/evidences', label: 'Repositorio Evidencias', icon: '📎' },
        { path: '/history', label: 'Historial y Trazabilidad', icon: '📜' },
      ]
    }
  ]

  const isActive = (path: string) => location.pathname === path

  return (
    <div style={{ display: 'flex', height: '100vh' }}>
      <nav style={{ width: '260px', backgroundColor: '#1a237e', color: 'white', padding: '1rem', display: 'flex', flexDirection: 'column', overflowY: 'auto' }}>
        <h2 style={{ marginBottom: '1.5rem', fontSize: '1.2rem' }}>Risk Governance Platform</h2>
        {menuSections.map(section => (
          <div key={section.label} style={{ marginBottom: '1.5rem' }}>
            <p style={{ fontSize: '0.75rem', textTransform: 'uppercase', color: '#7986cb', margin: '0 0 0.5rem 0', letterSpacing: '1px' }}>{section.label}</p>
            <ul style={{ listStyle: 'none', padding: 0, margin: 0 }}>
              {section.items.map(item => (
                <li key={item.path} style={{ marginBottom: '0.25rem' }}>
                  <Link
                    to={item.path}
                    style={{
                      display: 'flex',
                      alignItems: 'center',
                      gap: '8px',
                      padding: '8px 10px',
                      color: isActive(item.path) ? '#fff' : '#b0bec5',
                      textDecoration: 'none',
                      backgroundColor: isActive(item.path) ? 'rgba(255,255,255,0.1)' : 'transparent',
                      borderRadius: '6px',
                      fontSize: '0.9rem',
                      transition: 'all 0.2s'
                    }}
                  >
                    <span>{item.icon}</span>
                    <span>{item.label}</span>
                  </Link>
                </li>
              ))}
            </ul>
          </div>
        ))}
        <div style={{ marginTop: 'auto', paddingTop: '1rem', borderTop: '1px solid rgba(255,255,255,0.1)' }}>
          <p style={{ fontSize: '0.8rem', color: '#7986cb', margin: 0 }}>{accounts[0]?.name}</p>
        </div>
      </nav>
      <div style={{ flex: 1, display: 'flex', flexDirection: 'column' }}>
        <header style={{ padding: '10px 20px', borderBottom: '1px solid #e0e0e0', display: 'flex', justifyContent: 'space-between', alignItems: 'center', backgroundColor: '#fff' }}>
          <span style={{ fontWeight: 500 }}>Risk Governance Platform</span>
          <button
            onClick={handleLogout}
            style={{ padding: '6px 14px', backgroundColor: '#f44336', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', fontSize: '0.85rem' }}
          >
            Cerrar sesión
          </button>
        </header>
        <main style={{ flex: 1, padding: '1.5rem', overflow: 'auto', backgroundColor: '#f5f5f5' }}>
          {children}
        </main>
      </div>
    </div>
  )
}
