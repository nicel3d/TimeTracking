<template>
  <v-dialog v-model="dialog" max-width="500px">
    <v-card>
      <v-card-title>Обновление ограничений по программе</v-card-title>
      <v-divider></v-divider>
      <v-card-text v-if="item">
        <v-form :data-vv-scope="formId" @submit.prevent="onEdit">
          <v-flex>
            <v-text-field
              type="text"
              :value="item.caption"
              label="Приложение"
              readonly
            />
          </v-flex>

          <v-flex>
            <v-select
              v-model="state"
              :items="states"
              item-value="state"
              item-text="text"
              single-line
              ref="state"
              persistent-hint
              label="Статус"
            />
          </v-flex>
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
          @click.prevent="onEdit">Добавить
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Component, Inject, Vue } from 'vue-property-decorator'
import { Validator } from 'vee-validate'
import { Applications, StateEnum } from '%/stores/api/SwaggerDocumentationTypescript'
import { ApplicationEmitEnum } from '%/constants/WindowsEmmit'
import { States } from '%/constants/States'

@Component
export default class VWindowEditApplication extends Vue {
  @Inject('$validator') public $validator!: Validator

  id!: number
  item: Applications | null = null
  dialog: boolean = false
  $refs: any
  $options: any
  formId: string = 'form-application-group-edit'
  programs: object[] = []
  loading: boolean = false
  state: StateEnum = StateEnum.Neutral
  states = States

  onReset () {
    Object.assign(this.$data, this.$options.data.call(this))
  }

  mounted () {
    this.$root.$on(ApplicationEmitEnum.EDIT_APPLICATION, this.onOpenWindow)
  }

  onOpenWindow (item: Applications) {
    this.item = item
    this.id = item.id || 0
    this.state = item.state
    this.dialog = true
    setTimeout(() => this.$refs.state.focus(), 200)
  }

  onEdit () {
    this.$validator.validateAll(this.formId)
      .then((res) => {
        if (res && this.item) {
          this.$store.state.api.applications_PutState(this.id, this.state)
            .then(() => {
              this.$root.$emit(ApplicationEmitEnum.CHANGE_APPLICATION_SUCCESS)
              this.onReset()
            })
            .catch(res => this.$root.$emit('snackbar', res))
        }
      })
  }

  beforeDestroy () {
    this.$root.$off(ApplicationEmitEnum.EDIT_APPLICATION, this.onOpenWindow)
  }
}

</script>

<style scoped>

</style>
