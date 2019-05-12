import { Component, Vue } from 'vue-property-decorator'
import { StateEnum } from '%/stores/api/SwaggerDocumentationTypescript'
import { oc } from 'ts-optchain'
import { States } from '%/constants/States'

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
    return oc(States.find(item => item.state === state)).text(States[1].text)
  }
}
