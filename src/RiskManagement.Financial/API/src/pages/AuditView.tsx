import { useState } from 'react'

export default function AuditView() {
  const [selectedNorm, setSelectedNorm] = useState('ISO27001')

  const auditData = [
    { control: 'A.5.1', description: 'Políticas de seguridad', status: 'Compliant', owner: 'CISO' },
    { control: 'A.6.1', description: 'Roles y responsabilidades', status: 'Compliant', owner: 'RRHH' },
    { control: 'A.8.1', description: 'Activos de seguridad', status: 'Partial', owner: 'TI' },
    { control: 'A.9.1', description: 'Control de acceso', status: 'Compliant', owner: 'TI' },
    { control: 'A.12.1', description: 'Gestión de incidentes', status: 'Non-Compliant', owner: 'TI' },
  ]

  return (
    <div style={{ padding: '2rem' }}>
      <h2>Vista Audit-Ready por Norma</h2>

      <div style={{ marginBottom: '1rem' }}>
        <label style={{ marginRight: '8px' }}>Norma:</label>
        <select
          value={selectedNorm}
          onChange={(e) => setSelectedNorm(e.target.value)}
          style={{ padding: '8px', borderRadius: '4px', border: '1px solid #ddd' }}
        >
          <option value="ISO27001">ISO 27001</option>
          <option value="NIST">NIST CSF</option>
          <option value="COSO">COSO ERM</option>
        </select>
      </div>

      <table style={{ width: '100%', borderCollapse: 'collapse' }}>
        <thead>
          <tr style={{ borderBottom: '2px solid #ddd', backgroundColor: '#f5f5f5' }}>
            <th style={{ textAlign: 'left', padding: '12px' }}>Control</th>
            <th style={{ textAlign: 'left', padding: '12px' }}>Descripción</th>
            <th style={{ textAlign: 'left', padding: '12px' }}>Estado</th>
            <th style={{ textAlign: 'left', padding: '12px' }}>Responsable</th>
          </tr>
        </thead>
        <tbody>
          {auditData.map(item => (
            <tr key={item.control} style={{ borderBottom: '1px solid #eee' }}>
              <td style={{ padding: '12px' }}>{item.control}</td>
              <td style={{ padding: '12px' }}>{item.description}</td>
              <td style={{ padding: '12px' }}>
                <span style={{
                  padding: '4px 8px',
                  borderRadius: '4px',
                  backgroundColor: item.status === 'Compliant' ? '#e8f5e9' :
                    item.status === 'Partial' ? '#fff3e0' : '#fce4ec',
                  color: item.status === 'Compliant' ? '#2e7d32' :
                    item.status === 'Partial' ? '#e65100' : '#c62828'
                }}>
                  {item.status}
                </span>
              </td>
              <td style={{ padding: '12px' }}>{item.owner}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
