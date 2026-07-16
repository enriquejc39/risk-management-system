import { useEffect, useState } from 'react'

interface Question {
  id: string
  text: string
  questionType: string
  options: string | null
  order: number
  isRequired: boolean
}

interface QuestionnaireData {
  id: string
  name: string
  description: string
  areaId: string | null
  area: { name: string } | null
  questions: Question[]
}

interface AreaOption {
  id: string
  name: string
}

interface CategoryOption {
  id: string
  name: string
}

export default function Questionnaire() {
  const [questionnaires, setQuestionnaires] = useState<QuestionnaireData[]>([])
  const [areas, setAreas] = useState<AreaOption[]>([])
  const [categories, setCategories] = useState<CategoryOption[]>([])
  const [selectedQuestionnaire, setSelectedQuestionnaire] = useState<string>('')
  const [selectedArea, setSelectedArea] = useState<string>('')
  const [selectedCategory, setSelectedCategory] = useState<string>('')
  const [currentStep, setCurrentStep] = useState(0)
  const [answers, setAnswers] = useState<Record<string, string>>({})
  const [submitting, setSubmitting] = useState(false)
  const [submitted, setSubmitted] = useState(false)
  const [showChat, setShowChat] = useState(false)

  const currentQ = questionnaires.find(q => q.id === selectedQuestionnaire)
  const questions = currentQ?.questions.sort((a, b) => a.order - b.order) || []

  useEffect(() => {
    Promise.all([
      fetch('/api/questionnaires').then(r => r.json()),
      fetch('/api/catalog/areas').then(r => r.json()),
    ]).then(([q, a]) => {
      setQuestionnaires(q)
      setAreas(a)
    }).catch(console.error)
  }, [])

  useEffect(() => {
    if (selectedArea) {
      fetch(`/api/catalog/categories/${selectedArea}`)
        .then(r => r.json())
        .then(setCategories)
        .catch(console.error)
    }
  }, [selectedArea])

  const handleAreaChange = (areaId: string) => {
    setSelectedArea(areaId)
    setSelectedCategory('')
    const q = questionnaires.find(q => q.areaId === areaId || q.areaId === null)
    if (q) setSelectedQuestionnaire(q.id)
  }

  const handleAnswer = (value: string) => {
    const q = questions[currentStep]
    if (q) setAnswers(prev => ({ ...prev, [q.id]: value }))
  }

  const handleSubmit = async () => {
    if (!currentQ || !selectedArea || !selectedCategory) return
    setSubmitting(true)
    try {
      const res = await fetch('/api/questionnaires/submit', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          questionnaireId: currentQ.id,
          areaId: selectedArea,
          categoryId: selectedCategory,
          riskOwnerId: '00000000-0000-0000-0000-000000000000',
          answers: Object.fromEntries(questions.map(q => [q.id, answers[q.id] || '']))
        })
      })
      if (res.ok) setSubmitted(true)
      else alert('Error al enviar el cuestionario')
    } catch (err) {
      console.error('Error:', err)
      alert('Error de conexión')
    } finally {
      setSubmitting(false)
    }
  }

  if (submitted) {
    return (
      <div style={{ padding: '2rem', textAlign: 'center' }}>
        <h2>¡Riesgo registrado exitosamente!</h2>
        <p style={{ color: '#666', marginBottom: '1.5rem' }}>La matriz de riesgos se ha actualizado automáticamente.</p>
        <button onClick={() => { setSubmitted(false); setAnswers({}); setCurrentStep(0); setSelectedQuestionnaire(''); setSelectedArea('') }}
          style={{ padding: '10px 20px', backgroundColor: '#1976d2', color: 'white', border: 'none', borderRadius: '6px', cursor: 'pointer' }}>
          Identificar otro riesgo
        </button>
      </div>
    )
  }

  return (
    <div style={{ display: 'flex', height: 'calc(100vh - 80px)' }}>
      <div style={{ flex: showChat ? 1 : 1, padding: '1.5rem', overflow: 'auto', transition: 'flex 0.3s' }}>
        <h2 style={{ marginBottom: '0.5rem' }}>Identificación Guiada de Riesgos</h2>
        <p style={{ color: '#666', marginBottom: '1.5rem' }}>Responde las preguntas y la matriz se construirá automáticamente.</p>

        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem', marginBottom: '1.5rem' }}>
          <div>
            <label style={{ display: 'block', marginBottom: '0.25rem', fontWeight: 500, fontSize: '0.9rem' }}>Área</label>
            <select value={selectedArea} onChange={e => handleAreaChange(e.target.value)}
              style={{ width: '100%', padding: '10px', borderRadius: '6px', border: '1px solid #ddd' }}>
              <option value="">Seleccionar área...</option>
              {areas.map(a => <option key={a.id} value={a.id}>{a.name}</option>)}
            </select>
          </div>
          <div>
            <label style={{ display: 'block', marginBottom: '0.25rem', fontWeight: 500, fontSize: '0.9rem' }}>Categoría</label>
            <select value={selectedCategory} onChange={e => setSelectedCategory(e.target.value)}
              style={{ width: '100%', padding: '10px', borderRadius: '6px', border: '1px solid #ddd' }}>
              <option value="">Seleccionar categoría...</option>
              {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
            </select>
          </div>
        </div>

        {selectedQuestionnaire && questions.length > 0 && (
          <>
            <div style={{ marginBottom: '1rem' }}>
              <p style={{ fontSize: '0.9rem', fontWeight: 500 }}>{currentQ?.name}</p>
              <div style={{ height: '4px', backgroundColor: '#e0e0e0', borderRadius: '2px' }}>
                <div style={{ height: '100%', width: `${((currentStep + 1) / questions.length) * 100}%`, backgroundColor: '#1976d2', borderRadius: '2px', transition: 'width 0.3s' }} />
              </div>
              <p style={{ fontSize: '0.8rem', color: '#999' }}>Pregunta {currentStep + 1} de {questions.length}</p>
            </div>

            <div style={{ backgroundColor: 'white', borderRadius: '8px', padding: '1.5rem', boxShadow: '0 1px 3px rgba(0,0,0,0.1)', marginBottom: '1.5rem' }}>
              <label style={{ display: 'block', marginBottom: '0.75rem', fontWeight: 500, fontSize: '1.05rem' }}>
                {questions[currentStep]?.text}
                {questions[currentStep]?.isRequired && <span style={{ color: '#f44336' }}> *</span>}
              </label>
              {questions[currentStep]?.questionType === 'textarea' ? (
                <textarea value={answers[questions[currentStep]?.id] || ''} onChange={e => handleAnswer(e.target.value)}
                  style={{ width: '100%', minHeight: '100px', padding: '10px', borderRadius: '6px', border: '1px solid #ddd', fontSize: '0.95rem' }} />
              ) : questions[currentStep]?.questionType === 'select' && questions[currentStep]?.options ? (
                <div style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
                  {questions[currentStep].options.split('|').map(opt => (
                    <label key={opt} style={{ display: 'flex', alignItems: 'center', gap: '8px', padding: '10px', borderRadius: '6px', border: answers[questions[currentStep].id] === opt ? '2px solid #1976d2' : '1px solid #ddd', cursor: 'pointer', backgroundColor: answers[questions[currentStep].id] === opt ? '#e3f2fd' : 'white' }}>
                      <input type="radio" name={questions[currentStep].id} value={opt} checked={answers[questions[currentStep].id] === opt} onChange={e => handleAnswer(e.target.value)} />
                      <span>{opt}</span>
                    </label>
                  ))}
                </div>
              ) : (
                <input type="text" value={answers[questions[currentStep]?.id] || ''} onChange={e => handleAnswer(e.target.value)}
                  style={{ width: '100%', padding: '10px', borderRadius: '6px', border: '1px solid #ddd', fontSize: '0.95rem' }} />
              )}
            </div>

            <div style={{ display: 'flex', gap: '0.75rem' }}>
              <button onClick={() => setCurrentStep(prev => Math.max(0, prev - 1))} disabled={currentStep === 0}
                style={{ padding: '10px 20px', backgroundColor: '#757575', color: 'white', border: 'none', borderRadius: '6px', cursor: currentStep === 0 ? 'not-allowed' : 'pointer', opacity: currentStep === 0 ? 0.5 : 1 }}>
                Anterior
              </button>
              {currentStep === questions.length - 1 ? (
                <button onClick={handleSubmit} disabled={submitting || !selectedCategory}
                  style={{ padding: '10px 20px', backgroundColor: '#4caf50', color: 'white', border: 'none', borderRadius: '6px', cursor: 'pointer' }}>
                  {submitting ? 'Enviando...' : 'Enviar'}
                </button>
              ) : (
                <button onClick={() => setCurrentStep(prev => Math.min(questions.length - 1, prev + 1))}
                  style={{ padding: '10px 20px', backgroundColor: '#1976d2', color: 'white', border: 'none', borderRadius: '6px', cursor: 'pointer' }}>
                  Siguiente
                </button>
              )}
              <button onClick={() => setShowChat(!showChat)}
                style={{ padding: '10px 20px', backgroundColor: '#9c27b0', color: 'white', border: 'none', borderRadius: '6px', cursor: 'pointer' }}>
                {showChat ? 'Ocultar Risk Copilot' : 'Risk Copilot'}
              </button>
            </div>
          </>
        )}
      </div>

      {showChat && (
        <div style={{ width: '350px', borderLeft: '1px solid #ddd', padding: '1rem', backgroundColor: '#f9f9f9', display: 'flex', flexDirection: 'column' }}>
          <h3 style={{ marginBottom: '1rem' }}>Risk Copilot</h3>
          <div style={{ flex: 1, overflow: 'auto', marginBottom: '1rem' }}>
            <div style={{ padding: '10px', backgroundColor: '#e3f2fd', borderRadius: '8px', marginBottom: '8px', fontSize: '0.9rem' }}>
              Hola, soy Risk Copilot. Puedo ayudarte a identificar y evaluar riesgos. Pregúntame lo que necesites.
            </div>
          </div>
          <div style={{ display: 'flex', gap: '8px' }}>
            <input type="text" placeholder="Ej: ¿Qué controles recomiendas?" style={{ flex: 1, padding: '10px', borderRadius: '6px', border: '1px solid #ddd' }} />
            <button style={{ padding: '10px 14px', backgroundColor: '#1976d2', color: 'white', border: 'none', borderRadius: '6px', cursor: 'pointer' }}>Enviar</button>
          </div>
        </div>
      )}
    </div>
  )
}
