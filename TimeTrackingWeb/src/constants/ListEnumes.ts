import { ModeEnum, StateEnum } from '%/stores/api/SwaggerDocumentationTypescript'

const States = [
  {
    state: StateEnum.Allowed,
    text: 'Разрешено'
  },
  {
    state: StateEnum.Neutral,
    text: 'Не обработано'
  },
  {
    state: StateEnum.Forbidden,
    text: 'Запрещено'
  }
]

const Modes = [
  {
    state: ModeEnum.Contains,
    text: 'Содержит в заголовке текст'
  },
  {
    state: ModeEnum.Exactly,
    text: 'Соответствует тексту в заголовке'
  },
  {
    state: ModeEnum.DoesNotContain,
    text: 'Не включает текст в заголовке'
  }
]

export {
  States,
  Modes
}
