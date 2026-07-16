import { useEffect, useState } from 'react'

interface AuditLogEntry {
  id: string
  entityType: string
  entityId: string
  action: string
  userId: string
  userName: string
  oldValues: string | null
  newValues: string | null
  timestamp: string
  ipAddress: string | null
}

export default function History() {
  const [logs, setLogs] = useState<AuditLogEntry[]>([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchHistory = async () => {
      try {
        const res = await fetch('/api/auditlogs')
        if (res.ok) setLogs(await res.json())
        else {
          const res2 = await fetch('/api/reports/history/Risk/00000000-0000-0000-0000-000000000000')
          if (res2.ok) {
            const data = await res2.json()
            setLogs(data.length ? data : [])
          }
        }
      } catch (err) {
        console.error('Error fetching history:', err)
      } finally {
        setLoading(false)
      }
    }
    fetchHistory()
  }, [])

  const getActionColor = (action: string) => {
    switch (action) {
      case 'Created': return '#4caf50'
      case 'Updated': return '#2196f3'
      case 'Deleted': return '#f44336'
      case 'Approved': return '#66bb6a'
      case 'Rejected': return '#ef5350'
      case 'Submitted': return '#ff9800'
      default: return '#9e9e9e'
    }
  }

  return (
    <div>
      <h2 style={{ marginBottom: '1rem' }}>Historial y Trazabilidad</h2>
      <p style={{ color: '#666', marginBottom: '1.5rem' }}>
        Cada cambio queda registrado automáticamente para auditorías.
      </p>
      {loading ? (
        <p>Cargando historial...</p>
      ) : (
        <div style={{ backgroundColor: 'white', borderRadius: '8px', boxShadow: '0 1px 3px rgba(0,0,0,0.1)', overflow: 'hidden' }}>
          <table style={{ width: '100%', borderCollapse: 'collapse' }}>
            <thead>
              <tr style={{ backgroundColor: '#f5f5f5', borderBottom: '2px solid #e0e0e0' }}>
                <th style={{ padding: '12px', textAlign: 'left' }}>Fecha</th>
                <th style={{ padding: '12px', textAlign: 'left' }}>Usuario</th>
                <th style={{ padding: '12px', textAlign: 'left' }}>Entidad</th>
                <th style={{ padding: '12px', textAlign: 'center' }}>Acción</th>
                <th style={{ padding: '12px', textAlign: 'left' }}>Cambios</th>
              </tr>
            </thead>
            <tbody>
              {logs.map(log => (
                <tr key={log.id} style={{ borderBottom: '1px solid #eee' }}>
                  <td style={{ padding: '12px', fontSize: '0.85rem', whiteSpace: 'nowrap' }}>
                    {new Date(log.timestamp).toLocaleString()}
                  </td>
                  <td style={{ padding: '12px', fontSize: '0.9rem' }}>
                    {log.userName || log.userId}
                  </td>
                  <td style={{ padding: '12px', fontSize: '0.85rem' }}>
                    <span style={{ fontFamily: 'monospace', fontSize: '0.8rem' }}>{log.entityType}</span>
                    <br />
                    <span style={{ fontSize: '0.75rem', color: '#999' }}>{log.entityId.substring(0, 8)}</span>
                  </td>
                  <td style={{ padding: '12px', textAlign: 'center' }}>
                    <span style={{ padding: '3px 10px', borderRadius: '10px', backgroundColor: getActionColor(log.action), color: 'white', fontSize: '0.8rem' }}>
                      {log.action}
                    </span>
                  </td>
                  <td style={{ padding: '12px', fontSize: '0.8rem', color: '#555', maxWidth: '300px', overflow: 'hidden', textOverflow: 'ellipsis' }}>
                    {log.newValues ? `→ ${log.newValues.substring(0, 100)}` : '-'}
                  </td>
                </tr>
              ))}
              {logs.length === 0 && (
                <tr><td colSpan={5} style={{ padding: '2rem', textAlign: 'center', color: '#999' }}>No hay registros de auditoría aún.</td></tr>
              )}
            </tbody>
          </table>
        </div>
      )}
    </div>
  )
}
