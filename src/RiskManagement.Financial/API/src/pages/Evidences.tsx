import { useEffect, useState } from 'react'

interface EvidenceItem {
  id: string
  riskId: string
  fileName: string
  filePath: string
  fileSize: number
  contentType: string
  version: number
  uploadedAt: string
  uploadedBy: string
}

interface RiskOption {
  id: string
  name: string
}

export default function Evidences() {
  const [evidences, setEvidences] = useState<EvidenceItem[]>([])
  const [risks, setRisks] = useState<RiskOption[]>([])
  const [selectedRiskId, setSelectedRiskId] = useState('')
  const [loading, setLoading] = useState(false)

  useEffect(() => {
    fetch('/api/catalog/risks')
      .then(res => res.json())
      .then(data => setRisks(data))
      .catch(console.error)
  }, [])

  useEffect(() => {
    if (!selectedRiskId) return
    setLoading(true)
    fetch(`/api/catalog/evidences/${selectedRiskId}`)
      .then(res => res.json())
      .then(data => setEvidences(data))
      .catch(console.error)
      .finally(() => setLoading(false))
  }, [selectedRiskId])

  const formatSize = (bytes: number) => {
    if (bytes < 1024) return `${bytes} B`
    if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} KB`
    return `${(bytes / (1024 * 1024)).toFixed(1)} MB`
  }

  return (
    <div>
      <h2 style={{ marginBottom: '1rem' }}>Repositorio de Evidencias</h2>
      <p style={{ color: '#666', marginBottom: '1.5rem' }}>
        Evidencias asociadas a cada riesgo. Clave para preparación de auditorías ISO.
      </p>
      <div style={{ marginBottom: '1.5rem' }}>
        <label style={{ display: 'block', marginBottom: '0.5rem', fontWeight: 500 }}>Seleccionar Riesgo:</label>
        <select
          value={selectedRiskId}
          onChange={e => setSelectedRiskId(e.target.value)}
          style={{ width: '100%', maxWidth: '500px', padding: '10px', borderRadius: '6px', border: '1px solid #ddd' }}
        >
          <option value="">-- Seleccione un riesgo --</option>
          {risks.map(r => (
            <option key={r.id} value={r.id}>{r.name}</option>
          ))}
        </select>
      </div>
      {loading ? (
        <p>Cargando evidencias...</p>
      ) : selectedRiskId ? (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))', gap: '1rem' }}>
          {evidences.map(ev => (
            <div key={ev.id} style={{ backgroundColor: 'white', borderRadius: '8px', padding: '1rem', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}>
              <div style={{ display: 'flex', alignItems: 'center', gap: '8px', marginBottom: '0.5rem' }}>
                <span style={{ fontSize: '1.5rem' }}>📎</span>
                <div>
                  <p style={{ margin: 0, fontWeight: 500, fontSize: '0.9rem' }}>{ev.fileName}</p>
                  <p style={{ margin: 0, fontSize: '0.8rem', color: '#999' }}>{ev.contentType} · {formatSize(ev.fileSize)} · v{ev.version}</p>
                </div>
              </div>
              <div style={{ fontSize: '0.8rem', color: '#666' }}>
                <p style={{ margin: '0.25rem 0' }}>Subido por: {ev.uploadedBy}</p>
                <p style={{ margin: '0.25rem 0' }}>Fecha: {new Date(ev.uploadedAt).toLocaleString()}</p>
              </div>
            </div>
          ))}
          {evidences.length === 0 && (
            <p style={{ color: '#999' }}>Este riesgo no tiene evidencias asociadas.</p>
          )}
        </div>
      ) : (
        <p style={{ color: '#999' }}>Seleccione un riesgo para ver sus evidencias.</p>
      )}
    </div>
  )
}
