<template>
  <v-dialog-full-window
    @on-save="onSave"
    :item="item"
    :loading="loading"
    v-model="dialog">
    <span slot="title">Детали группы</span>
    <template>
      <v-form data-vv-scope="form-group-edit">
        <v-container fluid grid-list-md>
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
                :value="updatedAt"
              />
            </v-flex>
          </v-layout>
        </v-container>
      </v-form>
      <v-divider></v-divider>
    </template>
  </v-dialog-full-window>
</template>

<script lang="ts">
import { Component, Inject, Vue } from 'vue-property-decorator'
import { Validator } from 'vee-validate'
import { Groups } from '%/stores/api/SwaggerDocumentationTypescript'
import { GroupEmitEnum } from '%/constants/windows/GroupsWindows'
import VDialogFullWindow from '%/utils/VDialogFullWindow.vue'

@Component({
  components: { VDialogFullWindow }
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

  get updatedAt () {
    if (this.item && this.item.updatedAt) {
      return this.item.updatedAt.toLocaleDateString() + ' ' +
        this.item.updatedAt.toLocaleTimeString('ru', {
          hour: '2-digit',
          minute: '2-digit'
        })
    }

    return ''
  }

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
