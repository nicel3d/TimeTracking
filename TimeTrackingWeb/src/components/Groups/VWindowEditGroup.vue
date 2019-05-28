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
            <v-form :data-vv-scope="formId">
              <v-container v-if="this.item" fluid grid-list-md>
                <v-layout row wrap>
                  <v-flex xs12 md6>
                    <v-text-field
                      type="text"
                      label="Название группы"
                      data-vv-as="Название группы"
                      :data-vv-name="`${formId}.name`"
                      prepend-icon="fulcrum"
                      name="group-name"
                      @keyup.enter="onSave"
                      v-model="name"
                      v-validate="'required'"
                      :error-messages="errors.collect(`${formId}.name`)"
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
            <v-table-staff :group-id="item.id" v-if="item && item.id"/>
          </v-tab-item>
          <v-tab-item>
            <v-table-application-groups :group-id="item.id" v-if="item && item.id"/>
          </v-tab-item>
          <v-tab-item>
            <v-table-group-application-titles :group-id="item.id" v-if="item && item.id"/>
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
import VTableStaff from '%/components/Staff/VTableStaff.vue'
import VTableApplicationGroups from '%/components/ApplicationGroups/VTableApplicationGroups.vue'
import VTableGroupApplicationTitles from '%/components/GroupApplicationTitles/VTableGroupApplicationTitles.vue'

@Component({
  components: { VTableGroupApplicationTitles, VTableApplicationGroups, VTableStaff, VDialogFullWindow }
})
export default class VWindowEditGroup extends Vue {
  @Inject('$validator') public $validator!: Validator

  item: Groups | null = null
  dialog: boolean = false
  loading: boolean = true
  name: string = ''
  formId: string = 'form-group-edit'
  $refs: any
  $options: any
  id: number = 0
  tabs: number = 0
  tabsItems: string[] = [
    'Детали',
    'Пользователи в группе',
    'Обработка программ',
    'Обработка заголовков у программ'
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
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  onSave () {
    this.tabs = 0
    this.$validator.validateAll(this.formId)
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
