<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <v-dialog v-model="dialog" max-width="500px">
    <v-card>
      <v-card-title>Добавление ограничения по программам</v-card-title>
      <v-divider></v-divider>
      <v-card-text v-if="dialog">
        <v-form :data-vv-scope="formId" @submit.prevent="onAdd">
          <v-flex>
            <v-autocomplete
              v-model="applicationId"
              :items="programs"
              label="Программа"
              data-vv-as="Название программы"
              :data-vv-name="`${formId}.program`"
              item-text="value"
              item-value="name"
              persistent-hint
              :loading="loading"
              v-validate="'required'"
              :error-messages="errors.collect(`${formId}.program`)"
            />
          </v-flex>

          <v-flex>
            <v-select
              v-model="state"
              :items="states"
              item-value="state"
              item-text="text"
              single-line
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
          @click.prevent="onAdd">Добавить
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Component, Inject, Vue } from 'vue-property-decorator'
import { Validator } from 'vee-validate'
import {
  ApplicationToGroup,
  StateEnum,
  TableSortingByGroupIdRequest
} from '%/stores/api/SwaggerDocumentationTypescript'
import { ApplicationGroupEmitEnum, ApplicationsIdsAndGroupId } from '%/constants/WindowsEmmit'
import { States } from '%/constants/States'

@Component
export default class VWindowAddApplicationGroup extends Vue {
  @Inject('$validator') public $validator!: Validator

  dialog: boolean = false
  applicationsIds: number[] = []
  $refs: any
  $options: any
  formId: string = 'form-application-group-add'
  groupId: number = 0
  applicationId: number | null = null
  programs: object[] = []
  loading: boolean = false
  state: StateEnum = StateEnum.Neutral
  states = States

  onReset () {
    Object.assign(this.$data, this.$options.data.call(this))
  }

  mounted () {
    this.$root.$on(ApplicationGroupEmitEnum.ADD_APPLICATION_GROUP, this.onOpenWindow)
  }

  onOpenWindow (request: ApplicationsIdsAndGroupId) {
    const { groupId, applicationsIds } = request
    this.dialog = true
    this.groupId = groupId
    this.applicationsIds = applicationsIds
    this.loadApplications()
  }

  onAdd () {
    this.$validator.validateAll(this.formId)
      .then((res) => {
        if (res && this.applicationId) {
          const data = new ApplicationToGroup({
            applicationId: this.applicationId,
            groupId: this.groupId,
            state: this.state
          })
          this.$store.state.api.treatmentApplications_Post(data)
            .then(() => {
              this.$root.$emit(ApplicationGroupEmitEnum.CHANGE_APPLICATION_GROUP_SUCCESS)
              this.onReset()
            })
            .catch(res => this.$root.$emit('snackbar', res))
        }
      })
  }

  loadApplications () {
    this.loading = true
    this.$store.state.api.applications_GetListFull(new TableSortingByGroupIdRequest())
      .then(res => {
        this.programs = res
          .filter(x => !this.applicationsIds.includes(x.id))
          .map(x => ({
            name: x.id,
            value: x.caption
          }))
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  beforeDestroy () {
    this.$root.$off(ApplicationGroupEmitEnum.ADD_APPLICATION_GROUP, this.onOpenWindow)
  }
}

</script>

<style scoped>

</style>
