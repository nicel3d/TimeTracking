<template>
  <v-dialog-full-window
    @on-save="onSave"
    :item="item"
    :loading="loading"
    v-model="dialog">
    <span slot="title">Детали приложения: {{ item ? item.caption : 'не определено' }}</span>
    <template slot="extension">
      <v-tabs
        v-model="tabs"
        grow
        color="transparent"
        slider-color="white">
        <v-tab v-for="n in tabsItems" :key="n">
          {{ n }}
        </v-tab>
      </v-tabs>
    </template>
    <template>
      <div>
        <v-tabs-items v-model="tabs">
          <v-tab-item>
            <v-form :data-vv-scope="formId">
              <v-container v-if="this.item" fluid grid-list-md>
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
              </v-container>
            </v-form>
          </v-tab-item>
          <v-tab-item>
            <v-table-application-titles v-if="item" :application-id="item.id"/>
          </v-tab-item>
        </v-tabs-items>
      </div>
      <v-divider></v-divider>
    </template>
  </v-dialog-full-window>
</template>

<script lang="ts">
import { Component, Inject, Vue } from 'vue-property-decorator'
import { Validator } from 'vee-validate'
import { Applications, StateEnum } from '%/stores/api/SwaggerDocumentationTypescript'
import { ApplicationEmitEnum } from '%/constants/WindowsEmmit'
import { States } from '%/constants/States'
import VDialogFullWindow from '%/utils/VDialogFullWindow.vue'
import VTableApplicationTitles from '%/components/Applications/VTableApplicationTitles.vue'

@Component({
  components: { VTableApplicationTitles, VDialogFullWindow }
})
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
  tabs: number = 0
  tabsItems: string[] = [
    'Детали',
    'Заголовки приложений'
  ]

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

  onSave () {
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
