import { StateEnum } from '%/stores/api/SwaggerDocumentationTypescript'

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

export {
  States
}
