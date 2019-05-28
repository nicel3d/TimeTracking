import { Component, Vue } from 'vue-property-decorator'
import { ModeEnum, StateEnum } from '%/stores/api/SwaggerDocumentationTypescript'
import { oc } from 'ts-optchain'
import { Modes, States } from '%/constants/ListEnumes'

export function ConvertCurrentStateToRussian (state?: StateEnum) {
  return oc(States.find(item => item.state === state)).text(States[1].text)
}

@Component
export default class FieldsExtension extends Vue {
  GetUpdatedAt (dateString?: Date) {
    if (dateString) {
      return dateString.toLocaleDateString() + ' ' +
        dateString.toLocaleTimeString('ru', {
          hour: '2-digit',
          minute: '2-digit'
        })
    }

    return ''
  }

  GetCurrentState (state?: StateEnum) {
    return ConvertCurrentStateToRussian(state)
  }

  GetCurrentMode (state?: ModeEnum) {
    return oc(Modes.find(item => item.state === state)).text(Modes[1].text)
  }
}
