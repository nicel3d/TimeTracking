<template>
  <v-dialog-full-window
    @on-save="onSave"
    :item="item"
    :loading="loading"
    v-model="dialog">
    <span slot="title">Детали группы: {{ item ? item.name : 'не определено' }}</span>
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
            <v-form data-vv-scope="form-group-edit">
              <v-container v-if="this.item" fluid grid-list-md>
                <v-layout row wrap>
                  <v-flex xs12 md6>
                    <v-text-field
                      type="text"
                      label="Название группы"
                      data-vv-as="Название группы"
                      data-vv-name="form-group-edit.name"
                      prepend-icon="fulcrum"
                      name="group-name"
                      ref="name"
                      @keyup.enter="onSave"
                      v-model="name"
                      v-validate="'required'"
                      :error-messages="errors.collect('form-group-edit.name')"
                    />
                  </v-flex>

                  <v-flex xs12 md6>
                    <v-text-field
                      type="text"
                      readonly
                      label="Дата обновления"
                      :value="GetUpdatedAt(this.item.updatedAt)"
                    />
                  </v-flex>
                </v-layout>
              </v-container>
            </v-form>
          </v-tab-item>
          <v-tab-item>
            <v-staff-table-component :group-id="item.id" v-if="item && item.id"/>
          </v-tab-item>
          <v-tab-item>
            <v-application-group-table-component :group-id="item.id" v-if="item && item.id"/>
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
import { Groups } from '%/stores/api/SwaggerDocumentationTypescript'
import { GroupEmitEnum } from '%/constants/WindowsEmmit'
import VDialogFullWindow from '%/utils/VDialogFullWindow.vue'
import VStaffTableComponent from '%/components/Staff/VStaffTableComponent.vue'
import VApplicationGroupTableComponent from '%/components/ApplicationGroup/VApplicationGroupTableComponent.vue'

@Component({
  components: { VApplicationGroupTableComponent, VStaffTableComponent, VDialogFullWindow }
})
export default class VGroupsDetailsWindow extends Vue {
  @Inject('$validator') public $validator!: Validator

  item: Groups | null = null
  dialog: boolean = false
  loading: boolean = true
  name: string = ''
  $refs: any
  $options: any
  id: number = 0
  tabs: number = 0
  tabsItems: string[] = [
    'Детали',
    'Пользователи в группе',
    'Обработка программ'
  ]

  mounted () {
    this.$root.$on(GroupEmitEnum.EDIT_GROUP, this.onOpenWindow)
  }

  onReset () {
    this.item = null
    this.dialog = false
    this.loading = true
  }

  onOpenWindow (id: number) {
    this.id = id
    this.loadGroup()
  }

  loadGroup () {
    this.dialog = true
    this.loading = true
    this.$store.state.api.group_Get(this.id)
      .then(res => {
        this.item = res
        this.name = res.name
        setTimeout(() => this.$refs.name.focus(), 100)
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  onSave () {
    this.tabs = 0
    this.$validator.validateAll('form-group-edit')
      .then(res => {
        if (res) {
          const data = new Groups({
            ...this.item,
            name: this.name
          })
          this.$store.state.api.group_Put(this.id, data)
            .then(() => {
              this.$root.$emit(GroupEmitEnum.CHANGE_GROUP_SUCCESS)
              this.dialog = false
              this.onReset()
            })
            .catch(res => this.$root.$emit('snackbar', res))
        }
      })
  }

  beforeDestroy () {
    this.$root.$off(GroupEmitEnum.EDIT_GROUP, this.onOpenWindow)
  }
}
</script>

<style scoped>

</style>
