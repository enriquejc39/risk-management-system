import { useEffect, useState } from 'react'

interface RiskItem {
  id: string
  name: string
  description: string
  probability: number
  impact: number
  riskScore: number
  level: string
  status: string
  createdAt: string
  lastReviewDate: string | null
}

export default function RiskCatalog() {
  const [risks, setRisks] = useState<RiskItem[]>([])
  const [loading, setLoading] = useState(true)
  const [filter, setFilter] = useState('')

  useEffect(() => {
    const fetchCatalog = async () => {
      try {
        const res = await fetch('/api/catalog/risks')
        if (res.ok) setRisks(await res.json())
      } catch (err) {
        console.error('Error fetching catalog:', err)
      } finally {
        setLoading(false)
      }
    }
    fetchCatalog()
  }, [])

  const filtered = risks.filter(r =>
    r.name.toLowerCase().includes(filter.toLowerCase()) ||
    r.level.toLowerCase().includes(filter.toLowerCase())
  )

  const getLevelColor = (level: string) => {
    switch (level) {
      case 'Critical': return '#f44336'
      case 'High': return '#ff9800'
      case 'Medium': return '#ffc107'
      case 'Low': return '#8bc34a'
      default: return '#4caf50'
    }
  }

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Approved': return '#4caf50'
      case 'Submitted': return '#2196f3'
      case 'UnderReview': return '#ff9800'
      case 'Draft': return '#9e9e9e'
      case 'Mitigated': return '#66bb6a'
      case 'Closed': return '#78909c'
      default: return '#9e9e9e'
    }
  }

  return (
    <div>
      <h2 style={{ marginBottom: '1rem' }}>Catálogo Corporativo de Riesgos</h2>
      <input
        type="text"
        placeholder="Buscar por nombre o nivel..."
        value={filter}
        onChange={e => setFilter(e.target.value)}
        style={{ width: '100%', maxWidth: '400px', padding: '10px', borderRadius: '6px', border: '1px solid #ddd', marginBottom: '1rem' }}
      />
      {loading ? (
        <p>Cargando catálogo...</p>
      ) : (
        <div style={{ backgroundColor: 'white', borderRadius: '8px', boxShadow: '0 1px 3px rgba(0,0,0,0.1)', overflow: 'hidden' }}>
          <table style={{ width: '100%', borderCollapse: 'collapse' }}>
            <thead>
              <tr style={{ backgroundColor: '#f5f5f5', borderBottom: '2px solid #e0e0e0' }}>
                <th style={{ padding: '12px', textAlign: 'left' }}>ID</th>
                <th style={{ padding: '12px', textAlign: 'left' }}>Nombre</th>
                <th style={{ padding: '12px', textAlign: 'center' }}>Prob.</th>
                <th style={{ padding: '12px', textAlign: 'center' }}>Imp.</th>
                <th style={{ padding: '12px', textAlign: 'center' }}>Score</th>
                <th style={{ padding: '12px', textAlign: 'center' }}>Nivel</th>
                <th style={{ padding: '12px', textAlign: 'center' }}>Estado</th>
                <th style={{ padding: '12px', textAlign: 'center' }}>Última Revisión</th>
              </tr>
            </thead>
            <tbody>
              {filtered.map(risk => (
                <tr key={risk.id} style={{ borderBottom: '1px solid #eee' }}>
                  <td style={{ padding: '12px', fontFamily: 'monospace', fontSize: '0.85rem' }}>{risk.id.substring(0, 8)}</td>
                  <td style={{ padding: '12px', fontWeight: 500 }}>{risk.name}</td>
                  <td style={{ padding: '12px', textAlign: 'center' }}>{risk.probability}</td>
                  <td style={{ padding: '12px', textAlign: 'center' }}>{risk.impact}</td>
                  <td style={{ padding: '12px', textAlign: 'center', fontWeight: 'bold' }}>{risk.riskScore}</td>
                  <td style={{ padding: '12px', textAlign: 'center' }}>
                    <span style={{ padding: '4px 10px', borderRadius: '12px', backgroundColor: getLevelColor(risk.level), color: 'white', fontSize: '0.85rem', fontWeight: 500 }}>
                      {risk.level}
                    </span>
                  </td>
                  <td style={{ padding: '12px', textAlign: 'center' }}>
                    <span style={{ padding: '4px 10px', borderRadius: '12px', backgroundColor: getStatusColor(risk.status), color: 'white', fontSize: '0.85rem' }}>
                      {risk.status}
                    </span>
                  </td>
                  <td style={{ padding: '12px', textAlign: 'center', fontSize: '0.85rem', color: '#666' }}>
                    {risk.lastReviewDate ? new Date(risk.lastReviewDate).toLocaleDateString() : 'Sin revisión'}
                  </td>
                </tr>
              ))}
              {filtered.length === 0 && (
                <tr><td colSpan={8} style={{ padding: '2rem', textAlign: 'center', color: '#999' }}>No se encontraron riesgos</td></tr>
              )}
            </tbody>
          </table>
        </div>
      )}
    </div>
  )
}
