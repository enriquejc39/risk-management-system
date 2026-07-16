import { useEffect, useState } from 'react'
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts'

interface KpiData {
  totalRisks: number
  criticalRisks: number
  highRisks: number
  overdueRisks: number
  risksWithoutEvidence: number
  risksWithoutReview: number
  complianceRate: number
  averageResidualRisk: number
  overdueActionPlans: number
  completedActionPlans: number
  totalActionPlans: number
  topRisks: { riskName: string; score: number; level: string; area: string }[]
  madurezPorArea: { areaName: string; totalRisks: number; criticalRisks: number; averageScore: number; complianceRate: number; nivel: string }[]
}

const COLORS = ['#f44336', '#ff9800', '#ffc107', '#8bc34a', '#4caf50', '#2196f3', '#9c27b0', '#00bcd4']

export default function Dashboard() {
  const [data, setData] = useState<KpiData | null>(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchDashboard = async () => {
      try {
        const res = await fetch('/api/reports/dashboard')
        if (res.ok) setData(await res.json())
      } catch (err) {
        console.error('Error fetching dashboard:', err)
      } finally {
        setLoading(false)
      }
    }
    fetchDashboard()
  }, [])

  if (loading) return <p>Cargando dashboard...</p>
  if (!data) return <p>No se pudo cargar el dashboard.</p>

  const pieData = [
    { name: 'Críticos', value: data.criticalRisks },
    { name: 'Altos', value: data.highRisks },
    { name: 'Medios/Bajos', value: data.totalRisks - data.criticalRisks - data.highRisks },
  ]

  return (
    <div>
      <h2 style={{ marginBottom: '1.5rem' }}>Dashboard Ejecutivo</h2>
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(200px, 1fr))', gap: '1rem', marginBottom: '2rem' }}>
        <Card label="Total Riesgos" value={data.totalRisks} color="#1565c0" bg="#e3f2fd" />
        <Card label="Críticos" value={data.criticalRisks} color="#c62828" bg="#fce4ec" />
        <Card label="Altos" value={data.highRisks} color="#e65100" bg="#fff3e0" />
        <Card label="Riesgo Promedio" value={data.averageResidualRisk.toFixed(1)} color="#6a1b9a" bg="#f3e5f5" />
        <Card label="Planes Vencidos" value={data.overdueActionPlans} color="#d32f2f" bg="#ffebee" />
        <Card label="Cumplimiento" value={`${data.complianceRate.toFixed(0)}%`} color="#2e7d32" bg="#e8f5e9" />
        <Card label="Sin Evidencia" value={data.risksWithoutEvidence} color="#f57c00" bg="#fff3e0" />
        <Card label="Sin Revisión (90d)" value={data.risksWithoutReview} color="#546e7a" bg="#eceff1" />
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1.5rem', marginBottom: '2rem' }}>
        <div style={{ backgroundColor: 'white', borderRadius: '8px', padding: '1.25rem', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}>
          <h3 style={{ margin: '0 0 1rem 0', fontSize: '1rem' }}>Top 10 Riesgos</h3>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={data.topRisks.slice(0, 10)} layout="vertical" margin={{ left: 100 }}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis type="number" />
              <YAxis type="category" dataKey="riskName" width={90} tick={{ fontSize: 11 }} />
              <Tooltip />
              <Bar dataKey="score" fill="#1976d2" radius={[0, 4, 4, 0]} />
            </BarChart>
          </ResponsiveContainer>
        </div>
        <div style={{ backgroundColor: 'white', borderRadius: '8px', padding: '1.25rem', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}>
          <h3 style={{ margin: '0 0 1rem 0', fontSize: '1rem' }}>Distribución de Riesgos</h3>
          <ResponsiveContainer width="100%" height={300}>
            <PieChart>
              <Pie data={pieData} cx="50%" cy="50%" outerRadius={100} dataKey="value" label={({ name, value }) => `${name}: ${value}`}>
                {pieData.map((_, idx) => <Cell key={idx} fill={COLORS[idx]} />)}
              </Pie>
              <Tooltip />
            </PieChart>
          </ResponsiveContainer>
        </div>
      </div>

      <div style={{ backgroundColor: 'white', borderRadius: '8px', padding: '1.25rem', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}>
        <h3 style={{ margin: '0 0 1rem 0', fontSize: '1rem' }}>Madurez por Área</h3>
        <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr style={{ borderBottom: '2px solid #e0e0e0', backgroundColor: '#f5f5f5' }}>
              <th style={{ padding: '10px', textAlign: 'left' }}>Área</th>
              <th style={{ padding: '10px', textAlign: 'center' }}>Riesgos</th>
              <th style={{ padding: '10px', textAlign: 'center' }}>Críticos</th>
              <th style={{ padding: '10px', textAlign: 'center' }}>Score Prom.</th>
              <th style={{ padding: '10px', textAlign: 'center' }}>Cumplimiento</th>
              <th style={{ padding: '10px', textAlign: 'center' }}>Nivel</th>
            </tr>
          </thead>
          <tbody>
            {data.madurezPorArea.map(area => (
              <tr key={area.areaName} style={{ borderBottom: '1px solid #eee' }}>
                <td style={{ padding: '10px', fontWeight: 500 }}>{area.areaName}</td>
                <td style={{ padding: '10px', textAlign: 'center' }}>{area.totalRisks}</td>
                <td style={{ padding: '10px', textAlign: 'center', color: area.criticalRisks > 0 ? '#c62828' : 'inherit' }}>{area.criticalRisks}</td>
                <td style={{ padding: '10px', textAlign: 'center' }}>{area.averageScore.toFixed(1)}</td>
                <td style={{ padding: '10px', textAlign: 'center' }}>{area.complianceRate.toFixed(0)}%</td>
                <td style={{ padding: '10px', textAlign: 'center' }}>
                  <span style={{ padding: '3px 10px', borderRadius: '10px', backgroundColor: area.nivel === 'Crítico' ? '#fce4ec' : '#e8f5e9', color: area.nivel === 'Crítico' ? '#c62828' : '#2e7d32', fontSize: '0.8rem' }}>
                    {area.nivel}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}

function Card({ label, value, color, bg }: { label: string; value: string | number; color: string; bg: string }) {
  return (
    <div style={{ padding: '1.25rem', backgroundColor: bg, borderRadius: '8px' }}>
      <h3 style={{ margin: 0, color, fontSize: '1.75rem' }}>{value}</h3>
      <p style={{ margin: '0.25rem 0 0 0', color: '#666', fontSize: '0.85rem' }}>{label}</p>
    </div>
  )
}
