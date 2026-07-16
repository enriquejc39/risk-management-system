import { useEffect, useState } from 'react'

interface ActionPlanItem {
  id: string
  riskId: string
  action: string
  description: string
  responsible: string
  commitmentDate: string
  status: number
  progress: number
  observations: string
}

const statusLabels = ['Pendiente', 'En Progreso', 'Completado', 'Cancelado']
const statusColors = ['#ff9800', '#2196f3', '#4caf50', '#9e9e9e']

export default function ActionPlans() {
  const [plans, setPlans] = useState<ActionPlanItem[]>([])
  const [loading, setLoading] = useState(true)
  const [filter, setFilter] = useState('all')

  useEffect(() => {
    const fetchPlans = async () => {
      try {
        const endpoint = filter === 'overdue' ? '/api/actionplans/overdue' : '/api/actionplans'
        const res = await fetch(endpoint)
        if (res.ok) setPlans(await res.json())
      } catch (err) {
        console.error('Error fetching plans:', err)
      } finally {
        setLoading(false)
      }
    }
    fetchPlans()
  }, [filter])

  const isOverdue = (date: string) => new Date(date) < new Date()

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1rem' }}>
        <h2 style={{ margin: 0 }}>Planes de Acción</h2>
        <select
          value={filter}
          onChange={e => { setFilter(e.target.value); setLoading(true) }}
          style={{ padding: '8px', borderRadius: '6px', border: '1px solid #ddd' }}
        >
          <option value="all">Todos</option>
          <option value="overdue">Vencidos</option>
        </select>
      </div>
      {loading ? (
        <p>Cargando planes...</p>
      ) : (
        <div style={{ display: 'flex', flexDirection: 'column', gap: '0.75rem' }}>
          {plans.map(plan => (
            <div key={plan.id} style={{ backgroundColor: 'white', borderRadius: '8px', padding: '1.25rem', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'start', marginBottom: '0.5rem' }}>
                <div>
                  <h3 style={{ margin: 0, fontSize: '1rem' }}>{plan.action}</h3>
                  <span style={{ fontSize: '0.8rem', color: '#999', fontFamily: 'monospace' }}>Riesgo: {plan.riskId.substring(0, 8)}</span>
                </div>
                <div style={{ display: 'flex', gap: '8px', alignItems: 'center' }}>
                  <span style={{ padding: '4px 10px', borderRadius: '12px', backgroundColor: statusColors[plan.status], color: 'white', fontSize: '0.8rem' }}>
                    {statusLabels[plan.status]}
                  </span>
                  {isOverdue(plan.commitmentDate) && plan.status < 2 && (
                    <span style={{ padding: '4px 8px', borderRadius: '4px', backgroundColor: '#fce4ec', color: '#c62828', fontSize: '0.75rem', fontWeight: 500 }}>
                      VENCIDO
                    </span>
                  )}
                </div>
              </div>
              {plan.description && <p style={{ margin: '0 0 0.75rem 0', fontSize: '0.85rem', color: '#666' }}>{plan.description}</p>}
              <div style={{ display: 'flex', gap: '2rem', fontSize: '0.85rem', color: '#555', marginBottom: '0.75rem' }}>
                <span>👤 {plan.responsible}</span>
                <span>📅 Vence: {new Date(plan.commitmentDate).toLocaleDateString()}</span>
              </div>
              <div>
                <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.8rem', color: '#666', marginBottom: '4px' }}>
                  <span>Progreso</span>
                  <span>{plan.progress}%</span>
                </div>
                <div style={{ height: '6px', backgroundColor: '#e0e0e0', borderRadius: '3px' }}>
                  <div style={{ height: '100%', width: `${plan.progress}%`, backgroundColor: statusColors[plan.status], borderRadius: '3px', transition: 'width 0.3s' }} />
                </div>
              </div>
            </div>
          ))}
          {plans.length === 0 && <p style={{ color: '#999' }}>No hay planes de acción.</p>}
        </div>
      )}
    </div>
  )
}
