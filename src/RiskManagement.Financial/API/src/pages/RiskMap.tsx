export default function RiskMap() {
  const levels = [
    { label: 'Muy Bajo', color: '#4caf50', range: '1-4' },
    { label: 'Bajo', color: '#8bc34a', range: '5-9' },
    { label: 'Medio', color: '#ffc107', range: '10-14' },
    { label: 'Alto', color: '#ff9800', range: '15-19' },
    { label: 'Crítico', color: '#f44336', range: '20-25' },
  ]

  return (
    <div style={{ padding: '2rem' }}>
      <h2>Mapa de Calor de Riesgos</h2>
      <div style={{ display: 'flex', gap: '2rem' }}>
        <div>
          <h3>Probabilidad vs Impacto</h3>
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(6, 60px)', gap: '2px' }}>
            <div style={{ padding: '8px', backgroundColor: '#f5f5f5', textAlign: 'center', fontWeight: 'bold' }}>
              P \ I
            </div>
            {[1, 2, 3, 4, 5].map(i => (
              <div key={`header-${i}`} style={{ padding: '8px', backgroundColor: '#f5f5f5', textAlign: 'center', fontWeight: 'bold' }}>
                {i}
              </div>
            ))}

            {[5, 4, 3, 2, 1].map(prob => (
              <>
                <div key={`prob-${prob}`} style={{ padding: '8px', backgroundColor: '#f5f5f5', textAlign: 'center', fontWeight: 'bold' }}>
                  {prob}
                </div>
                {[1, 2, 3, 4, 5].map(impact => {
                  const score = prob * impact
                  let bgColor = '#4caf50'
                  if (score >= 20) bgColor = '#f44336'
                  else if (score >= 15) bgColor = '#ff9800'
                  else if (score >= 10) bgColor = '#ffc107'
                  else if (score >= 5) bgColor = '#8bc34a'

                  return (
                    <div
                      key={`${prob}-${impact}`}
                      style={{
                        padding: '8px',
                        backgroundColor: bgColor,
                        textAlign: 'center',
                        color: score >= 15 ? 'white' : 'black',
                        fontWeight: 'bold',
                        borderRadius: '4px'
                      }}
                    >
                      {score}
                    </div>
                  )
                })}
              </>
            ))}
          </div>
        </div>

        <div>
          <h3>Leyenda</h3>
          {levels.map(level => (
            <div key={level.label} style={{ display: 'flex', alignItems: 'center', gap: '8px', marginBottom: '8px' }}>
              <div style={{ width: '20px', height: '20px', backgroundColor: level.color, borderRadius: '4px' }} />
              <span>{level.label} ({level.range})</span>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}
