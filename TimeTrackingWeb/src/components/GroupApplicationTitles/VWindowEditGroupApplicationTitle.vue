<template>
  <v-dialog v-model="dialog" max-width="500px">
    <v-card>
      <v-card-title>Обновление обработчики для заголовка программы</v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        <v-form :data-vv-scope="formId">
          <v-container v-if="item" fluid grid-list-md>
            <v-flex>
              <v-autocomplete
                :value="item.applicationId"
                :items="programs"
                label="Программа"
                item-text="value"
                item-value="name"
                persistent-hint
                readonly
              />
            </v-flex>

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
                required
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
import {
  ApplicationTitleToGroup,
  ModeEnum,
  StateEnum,
  TableSortingByGroupIdRequest
} from '%/stores/api/SwaggerDocumentationTypescript'
import { GroupApplicationTitleEmitEnum } from '%/constants/WindowsEmmit'
import { Modes, States } from '%/constants/ListEnumes'
import VDialogFullWindow from '%/utils/VDialogFullWindow.vue'
import VTableApplicationTitles from '%/components/ApplicationTitles/VTableApplicationTitles.vue'

@Component({
  components: { VTableApplicationTitles, VDialogFullWindow }
})
export default class VWindowEditGroupApplicationTitle extends Vue {
  @Inject('$validator') public $validator!: Validator

  applicationId: number = 0
  dialog: boolean = false
  $refs: any
  $options: any
  programs: object[] = []
  applicationsIds: number[] = []
  formId: string = 'form-group-application-title-add'
  loading: boolean = false
  id: number = 0
  item: ApplicationTitleToGroup | null = null
  state: StateEnum = StateEnum.Neutral
  states = States
  mode: ModeEnum = ModeEnum.Exactly
  modes = Modes

  created () {
    this.loadApplications()
  }

  onReset () {
    Object.assign(this.$data, this.$options.data.call(this))
  }

  mounted () {
    this.$root.$on(GroupApplicationTitleEmitEnum.EDIT_GROUP_APPLICATION_TITLE, this.onOpenWindow)
  }

  onOpenWindow (item: ApplicationTitleToGroup) {
    this.item = item
    this.id = item.id || 0
    this.state = item.state
    this.mode = item.mode
    this.dialog = true
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

  onSave () {
    this.$validator.validateAll(this.formId)
      .then((res) => {
        if (res && this.item) {
          const data = new ApplicationTitleToGroup({
            ...this.item,
            state: this.state,
            mode: this.mode
          })
          this.$store.state.api.groupApplicationTitles_Put(this.id, data)
            .then(() => {
              this.$root.$emit(GroupApplicationTitleEmitEnum.CHANGE_GROUP_APPLICATION_TITLE)
              this.onReset()
            })
            .catch(res => this.$root.$emit('snackbar', res))
        }
      })
  }

  beforeDestroy () {
    this.$root.$off(GroupApplicationTitleEmitEnum.EDIT_GROUP_APPLICATION_TITLE, this.onOpenWindow)
  }
}

</script>

<style scoped>

</style>
