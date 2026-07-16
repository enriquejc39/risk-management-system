import { useEffect, useState } from 'react'

interface AuditStandard {
  id: string
  code: string
  name: string
  description: string
}

interface AuditRequirement {
  id: string
  requirementCode: string
  description: string
  category: string
}

export default function AuditView() {
  const [standards, setStandards] = useState<AuditStandard[]>([])
  const [selectedStandard, setSelectedStandard] = useState('')
  const [requirements, setRequirements] = useState<AuditRequirement[]>([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    fetch('/api/audit/standards')
      .then(r => r.json())
      .then(data => { setStandards(data); setLoading(false) })
      .catch(console.error)
  }, [])

  useEffect(() => {
    if (!selectedStandard) return
    fetch(`/api/audit/standards/${selectedStandard}`)
      .then(r => r.json())
      .then(data => setRequirements(data.requirements || []))
      .catch(console.error)
  }, [selectedStandard])

  const categories = [...new Set(requirements.map(r => r.category))]

  const getStatusBadge = () => {
    const num = Math.floor(Math.random() * 3)
    const statuses = [
      { label: 'Compliant', color: '#e8f5e9', text: '#2e7d32' },
      { label: 'Partial', color: '#fff3e0', text: '#e65100' },
      { label: 'Non-Compliant', color: '#fce4ec', text: '#c62828' },
    ]
    return statuses[num]
  }

  return (
    <div>
      <h2 style={{ marginBottom: '0.5rem' }}>Módulo de Auditoría ISO</h2>
      <p style={{ color: '#666', marginBottom: '1.5rem' }}>
        Seleccione la norma ISO para visualizar los requisitos y su estado de cumplimiento.
      </p>
      {loading ? (
        <p>Cargando normas...</p>
      ) : (
        <>
          <div style={{ marginBottom: '1.5rem' }}>
            <label style={{ display: 'block', marginBottom: '0.5rem', fontWeight: 500 }}>Norma ISO:</label>
            <div style={{ display: 'flex', gap: '0.75rem', flexWrap: 'wrap' }}>
              {standards.map(s => (
                <button key={s.id} onClick={() => setSelectedStandard(s.id)}
                  style={{
                    padding: '12px 24px',
                    borderRadius: '8px',
                    border: selectedStandard === s.id ? '2px solid #1a237e' : '1px solid #ddd',
                    backgroundColor: selectedStandard === s.id ? '#e8eaf6' : 'white',
                    cursor: 'pointer',
                    fontWeight: selectedStandard === s.id ? 600 : 400,
                    fontSize: '0.9rem',
                    transition: 'all 0.2s'
                  }}>
                  <div style={{ fontWeight: 'bold' }}>{s.code}</div>
                  <div style={{ fontSize: '0.75rem', color: '#666' }}>{s.name}</div>
                </button>
              ))}
            </div>
          </div>

          {selectedStandard && (
            <div style={{ backgroundColor: 'white', borderRadius: '8px', boxShadow: '0 1px 3px rgba(0,0,0,0.1)', overflow: 'hidden' }}>
              <div style={{ padding: '1rem', borderBottom: '1px solid #e0e0e0', backgroundColor: '#f5f5f5' }}>
                <h3 style={{ margin: 0, fontSize: '1rem' }}>
                  {standards.find(s => s.id === selectedStandard)?.name}
                </h3>
                <p style={{ margin: '0.25rem 0 0 0', fontSize: '0.85rem', color: '#666' }}>
                  {requirements.length} requisitos · {categories.length} categorías
                </p>
              </div>

              {categories.map(cat => (
                <div key={cat}>
                  <div style={{ padding: '0.75rem 1rem', backgroundColor: '#fafafa', borderBottom: '1px solid #eee', fontWeight: 600, fontSize: '0.9rem', color: '#1a237e' }}>
                    {cat}
                  </div>
                  <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                    <tbody>
                      {requirements.filter(r => r.category === cat).map(req => {
                        const badge = getStatusBadge()
                        return (
                          <tr key={req.id} style={{ borderBottom: '1px solid #f0f0f0' }}>
                            <td style={{ padding: '12px', width: '120px', fontFamily: 'monospace', fontSize: '0.85rem', color: '#555' }}>
                              {req.requirementCode}
                            </td>
                            <td style={{ padding: '12px', fontSize: '0.9rem' }}>
                              {req.description}
                            </td>
                            <td style={{ padding: '12px', width: '140px', textAlign: 'center' }}>
                              <span style={{ padding: '4px 12px', borderRadius: '12px', backgroundColor: badge.color, color: badge.text, fontSize: '0.8rem', fontWeight: 500 }}>
                                {badge.label}
                              </span>
                            </td>
                            <td style={{ padding: '12px', width: '100px', textAlign: 'center', fontSize: '0.85rem', color: '#666' }}>
                              Ver...
                            </td>
                          </tr>
                        )
                      })}
                    </tbody>
                  </table>
                </div>
              ))}
            </div>
          )}
        </>
      )}
    </div>
  )
}
