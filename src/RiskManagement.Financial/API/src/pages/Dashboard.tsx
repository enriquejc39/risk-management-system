import { useEffect, useState } from 'react'
import { useMsal } from '@azure/msal-react'
import { loginRequest } from '../authConfig'

interface Risk {
  id: string
  name: string
  probability: number
  impact: number
  riskScore: number
  level: string
  status: string
}

export default function Dashboard() {
  const { instance, accounts } = useMsal()
  const [risks, setRisks] = useState<Risk[]>([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchRisks = async () => {
      try {
        const tokenResponse = await instance.acquireTokenSilent({
          scopes: ['user.read'],
          account: accounts[0],
        })

        const response = await fetch('/api/risks', {
          headers: {
            Authorization: `Bearer ${tokenResponse.accessToken}`,
          },
        })

        if (response.ok) {
          const data = await response.json()
          setRisks(data)
        }
      } catch (error) {
        console.error('Error fetching risks:', error)
      } finally {
        setLoading(false)
      }
    }

    fetchRisks()
  }, [instance, accounts])

  const criticalRisks = risks.filter(r => r.level === 'Critical')
  const highRisks = risks.filter(r => r.level === 'High')

  return (
    <div>
      <h2>Dashboard Ejecutivo</h2>

      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(4, 1fr)', gap: '1rem', marginBottom: '2rem' }}>
        <div style={{ padding: '1rem', backgroundColor: '#e3f2fd', borderRadius: '8px' }}>
          <h3 style={{ margin: 0, color: '#1565c0' }}>{risks.length}</h3>
          <p style={{ margin: 0 }}>Total Riesgos</p>
        </div>
        <div style={{ padding: '1rem', backgroundColor: '#fce4ec', borderRadius: '8px' }}>
          <h3 style={{ margin: 0, color: '#c62828' }}>{criticalRisks.length}</h3>
          <p style={{ margin: 0 }}>Críticos</p>
        </div>
        <div style={{ padding: '1rem', backgroundColor: '#fff3e0', borderRadius: '8px' }}>
          <h3 style={{ margin: 0, color: '#e65100' }}>{highRisks.length}</h3>
          <p style={{ margin: 0 }}>Altos</p>
        </div>
        <div style={{ padding: '1rem', backgroundColor: '#e8f5e9', borderRadius: '8px' }}>
          <h3 style={{ margin: 0, color: '#2e7d32' }}>
            {risks.filter(r => r.status === 'Mitigated').length}
          </h3>
          <p style={{ margin: 0 }}>Mitigados</p>
        </div>
      </div>

      {loading ? (
        <p>Cargando riesgos...</p>
      ) : (
        <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr style={{ borderBottom: '2px solid #ddd' }}>
              <th style={{ textAlign: 'left', padding: '8px' }}>Nombre</th>
              <th style={{ textAlign: 'left', padding: '8px' }}>Probabilidad</th>
              <th style={{ textAlign: 'left', padding: '8px' }}>Impacto</th>
              <th style={{ textAlign: 'left', padding: '8px' }}>Score</th>
              <th style={{ textAlign: 'left', padding: '8px' }}>Nivel</th>
              <th style={{ textAlign: 'left', padding: '8px' }}>Estado</th>
            </tr>
          </thead>
          <tbody>
            {risks.map(risk => (
              <tr key={risk.id} style={{ borderBottom: '1px solid #eee' }}>
                <td style={{ padding: '8px' }}>{risk.name}</td>
                <td style={{ padding: '8px' }}>{risk.probability}</td>
                <td style={{ padding: '8px' }}>{risk.impact}</td>
                <td style={{ padding: '8px' }}>{risk.riskScore}</td>
                <td style={{ padding: '8px' }}>{risk.level}</td>
                <td style={{ padding: '8px' }}>{risk.status}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  )
}
