import { useEffect, useState } from 'react'

interface ControlItem {
  id: string
  name: string
  description: string
  controlType: string
  frequency: string
  responsibleArea: string
  isActive: boolean
}

export default function Controls() {
  const [controls, setControls] = useState<ControlItem[]>([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchControls = async () => {
      try {
        const res = await fetch('/api/controls/active')
        if (res.ok) setControls(await res.json())
      } catch (err) {
        console.error('Error fetching controls:', err)
      } finally {
        setLoading(false)
      }
    }
    fetchControls()
  }, [])

  const getTypeColor = (type: string) => {
    switch (type) {
      case 'Preventivo': return '#1976d2'
      case 'Detectivo': return '#f57c00'
      case 'Correctivo': return '#388e3c'
      default: return '#757575'
    }
  }

  return (
    <div>
      <h2 style={{ marginBottom: '1rem' }}>Biblioteca de Controles</h2>
      <p style={{ color: '#666', marginBottom: '1.5rem' }}>
        Catálogo centralizado de controles disponibles para mitigación de riesgos.
      </p>
      {loading ? (
        <p>Cargando controles...</p>
      ) : (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(350px, 1fr))', gap: '1rem' }}>
          {controls.map(control => (
            <div key={control.id} style={{ backgroundColor: 'white', borderRadius: '8px', padding: '1.25rem', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'start', marginBottom: '0.75rem' }}>
                <h3 style={{ margin: 0, fontSize: '1rem' }}>{control.name}</h3>
                <span style={{ padding: '3px 8px', borderRadius: '10px', backgroundColor: getTypeColor(control.controlType), color: 'white', fontSize: '0.75rem', fontWeight: 500 }}>
                  {control.controlType}
                </span>
              </div>
              <p style={{ margin: '0 0 0.75rem 0', fontSize: '0.85rem', color: '#666' }}>{control.description}</p>
              <div style={{ display: 'flex', gap: '1rem', fontSize: '0.85rem', color: '#555' }}>
                <span>🔄 {control.frequency}</span>
                <span>🏢 {control.responsibleArea}</span>
              </div>
            </div>
          ))}
          {controls.length === 0 && (
            <p style={{ color: '#999' }}>No hay controles registrados.</p>
          )}
        </div>
      )}
    </div>
  )
}
