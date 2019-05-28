<template>
  <v-dialog v-model="dialog" max-width="500px">
    <v-card>
      <v-card-title>Обновление обработчики для заголовка программы</v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        <v-form :data-vv-scope="formId">
          <v-container v-if="item" fluid grid-list-md>
            <v-flex>
              <v-select
                v-model="mode"
                :items="modes"
                item-value="state"
                item-text="text"
                persistent-hint
                label="Мод"
              />
            </v-flex>

            <v-flex>
              <v-text-field
                type="text"
                :value="item.title"
                label="Текст"
                readonly
              />
            </v-flex>

            <v-flex>
              <v-select
                v-model="state"
                :items="states"
                item-value="state"
                item-text="text"
                persistent-hint
                label="Статус"
              />
            </v-flex>
          </v-container>
        </v-form>
      </v-card-text>
      <v-divider></v-divider>
      <v-card-actions>
        <v-spacer/>
        <v-btn
          color="blue darken-1"
          flat
          @click.prevent="onReset">Отменить
        </v-btn>
        <v-btn
          color="primary"
          @click.prevent="onSave">Обновить
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Component, Inject, Vue } from 'vue-property-decorator'
import { Validator } from 'vee-validate'
import { ApplicationTitles, ModeEnum, StateEnum } from '%/stores/api/SwaggerDocumentationTypescript'
import { ApplicationTitleEmitEnum } from '%/constants/WindowsEmmit'
import { Modes, States } from '%/constants/ListEnumes'
import VDialogFullWindow from '%/utils/VDialogFullWindow.vue'
import VTableApplicationTitles from '%/components/ApplicationTitles/VTableApplicationTitles.vue'

@Component({
  components: { VTableApplicationTitles, VDialogFullWindow }
})
export default class VWindowEditApplicationTitle extends Vue {
  @Inject('$validator') public $validator!: Validator

  dialog: boolean = false
  $refs: any
  $options: any
  formId: string = 'form-application-title-edit'
  loading: boolean = false
  id: number = 0
  item: ApplicationTitles | null = null
  state: StateEnum = StateEnum.Neutral
  states = States
  mode: ModeEnum = ModeEnum.Exactly
  modes = Modes

  onReset () {
    Object.assign(this.$data, this.$options.data.call(this))
  }

  mounted () {
    this.$root.$on(ApplicationTitleEmitEnum.EDIT_APPLICATION_TITLE, this.onOpenWindow)
  }

  onOpenWindow (item: ApplicationTitles) {
    this.item = item || null
    this.id = item.id || 0
    this.state = item.state
    this.mode = item.mode
    this.dialog = true
  }

  onSave () {
    this.$validator.validateAll(this.formId)
      .then((res) => {
        if (res && this.item) {
          const data = new ApplicationTitles({
            ...this.item,
            state: this.state,
            mode: this.mode
          })
          this.$store.state.api.applicationTitles_Put(this.id, data)
            .then(() => {
              this.$root.$emit(ApplicationTitleEmitEnum.CHANGE_APPLICATION_TITLE)
              this.onReset()
            })
            .catch(res => this.$root.$emit('snackbar', res))
        }
      })
  }

  beforeDestroy () {
    this.$root.$off(ApplicationTitleEmitEnum.EDIT_APPLICATION_TITLE, this.onOpenWindow)
  }
}

</script>

<style scoped>

</style>
