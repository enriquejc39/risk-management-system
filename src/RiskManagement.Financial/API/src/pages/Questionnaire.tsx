import { useState } from 'react'

interface Question {
  id: string
  text: string
  type: 'text' | 'number' | 'select' | 'textarea'
  options?: string[]
}

const questions: Question[] = [
  { id: 'name', text: '¿Cuál es el nombre del riesgo?', type: 'text' },
  { id: 'description', text: 'Describe el riesgo identificado', type: 'textarea' },
  { id: 'probability', text: 'Probabilidad (1-5)', type: 'select', options: ['1', '2', '3', '4', '5'] },
  { id: 'impact', text: 'Impacto (1-5)', type: 'select', options: ['1', '2', '3', '4', '5'] },
  { id: 'area', text: '¿A qué área pertenece?', type: 'text' },
  { id: 'category', text: 'Categoría del riesgo', type: 'text' },
  { id: 'owner', text: 'Responsable del riesgo', type: 'text' },
]

export default function Questionnaire() {
  const [currentStep, setCurrentStep] = useState(0)
  const [answers, setAnswers] = useState<Record<string, string>>({})
  const [showChat, setShowChat] = useState(false)

  const currentQuestion = questions[currentStep]

  const handleAnswer = (value: string) => {
    setAnswers(prev => ({ ...prev, [currentQuestion.id]: value }))
  }

  const handleNext = () => {
    if (currentStep < questions.length - 1) {
      setCurrentStep(prev => prev + 1)
    }
  }

  const handlePrevious = () => {
    if (currentStep > 0) {
      setCurrentStep(prev => prev - 1)
    }
  }

  const handleSubmit = () => {
    console.log('Submitting risk:', answers)
    alert('Riesgo registrado exitosamente')
  }

  return (
    <div style={{ display: 'flex', height: 'calc(100vh - 60px)' }}>
      <div style={{ flex: showChat ? 1 : 1, padding: '2rem', transition: 'flex 0.3s' }}>
        <h2>Cuestionario de Evaluación de Riesgos</h2>
        <p>Paso {currentStep + 1} de {questions.length}</p>

        <div style={{ marginBottom: '1rem' }}>
          <div style={{ height: '4px', backgroundColor: '#e0e0e0', borderRadius: '2px' }}>
            <div style={{
              height: '100%',
              width: `${((currentStep + 1) / questions.length) * 100}%`,
              backgroundColor: '#1976d2',
              borderRadius: '2px',
              transition: 'width 0.3s'
            }} />
          </div>
        </div>

        <div style={{ marginBottom: '2rem' }}>
          <label style={{ display: 'block', marginBottom: '0.5rem', fontWeight: 'bold' }}>
            {currentQuestion.text}
          </label>
          {currentQuestion.type === 'textarea' ? (
            <textarea
              value={answers[currentQuestion.id] || ''}
              onChange={(e) => handleAnswer(e.target.value)}
              style={{ width: '100%', minHeight: '100px', padding: '8px', borderRadius: '4px', border: '1px solid #ddd' }}
            />
          ) : currentQuestion.type === 'select' ? (
            <select
              value={answers[currentQuestion.id] || ''}
              onChange={(e) => handleAnswer(e.target.value)}
              style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ddd' }}
            >
              <option value="">Seleccionar...</option>
              {currentQuestion.options?.map(opt => (
                <option key={opt} value={opt}>{opt}</option>
              ))}
            </select>
          ) : (
            <input
              type={currentQuestion.type}
              value={answers[currentQuestion.id] || ''}
              onChange={(e) => handleAnswer(e.target.value)}
              style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ddd' }}
            />
          )}
        </div>

        <div style={{ display: 'flex', gap: '1rem' }}>
          <button
            onClick={handlePrevious}
            disabled={currentStep === 0}
            style={{ padding: '8px 16px', backgroundColor: '#757575', color: 'white', border: 'none', borderRadius: '4px', cursor: currentStep === 0 ? 'not-allowed' : 'pointer' }}
          >
            Anterior
          </button>
          {currentStep === questions.length - 1 ? (
            <button
              onClick={handleSubmit}
              style={{ padding: '8px 16px', backgroundColor: '#4caf50', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}
            >
              Enviar
            </button>
          ) : (
            <button
              onClick={handleNext}
              style={{ padding: '8px 16px', backgroundColor: '#1976d2', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}
            >
              Siguiente
            </button>
          )}
          <button
            onClick={() => setShowChat(!showChat)}
            style={{ padding: '8px 16px', backgroundColor: '#9c27b0', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}
          >
            {showChat ? 'Ocultar Chat' : 'Risk Copilot'}
          </button>
        </div>
      </div>

      {showChat && (
        <div style={{ width: '350px', borderLeft: '1px solid #ddd', padding: '1rem', backgroundColor: '#f9f9f9' }}>
          <h3>Risk Copilot</h3>
          <div style={{ height: 'calc(100% - 100px)', overflow: 'auto', marginBottom: '1rem' }}>
            <div style={{ padding: '8px', backgroundColor: '#e3f2fd', borderRadius: '8px', marginBottom: '8px' }}>
              Hola, soy Risk Copilot. Puedo ayudarte a responder las preguntas del cuestionario.
            </div>
          </div>
          <div style={{ display: 'flex', gap: '8px' }}>
            <input
              type="text"
              placeholder="Escribe tu pregunta..."
              style={{ flex: 1, padding: '8px', borderRadius: '4px', border: '1px solid #ddd' }}
            />
            <button style={{ padding: '8px 12px', backgroundColor: '#1976d2', color: 'white', border: 'none', borderRadius: '4px' }}>
              Enviar
            </button>
          </div>
        </div>
      )}
    </div>
  )
}
