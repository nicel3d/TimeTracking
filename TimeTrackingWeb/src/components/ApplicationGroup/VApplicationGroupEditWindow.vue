<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <v-dialog v-model="dialog" max-width="500px">
    <v-card>
      <v-card-title>Обновление ограничения по программе</v-card-title>
      <v-divider></v-divider>
      <v-card-text v-if="item">
        <v-form :data-vv-scope="formId" @submit.prevent="onEdit">
          <v-flex>
            <v-text-field
              type="text"
              :value="item.applicationTitle"
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
import {
  ApplicationToGroup, StateEnum, VMApplicationGroup
} from '%/stores/api/SwaggerDocumentationTypescript'
import { ApplicationGroupEmitEnum } from '%/constants/WindowsEmmit'
import { States } from '%/constants/States'

@Component
export default class VGroupsAddWindow extends Vue {
  @Inject('$validator') public $validator!: Validator

  id!: number
  item: VMApplicationGroup | null = null
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
    this.$root.$on(ApplicationGroupEmitEnum.EDIT_APPLICATION_GROUP, this.onOpenWindow)
  }

  onOpenWindow (item: VMApplicationGroup) {
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
          const data = new ApplicationToGroup({
            ...this.item,
            state: this.state
          })
          this.$store.state.api.treatmentApplications_Put(this.id, data)
            .then(() => {
              this.$root.$emit(ApplicationGroupEmitEnum.CHANGE_APPLICATION_GROUP_SUCCESS)
              this.onReset()
            })
            .catch(res => this.$root.$emit('snackbar', res))
        }
      })
  }

  beforeDestroy () {
    this.$root.$off(ApplicationGroupEmitEnum.EDIT_APPLICATION_GROUP, this.onOpenWindow)
  }
}

</script>

<style scoped>

</style>
