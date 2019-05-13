<template>
  <v-dialog v-model="dialog" max-width="300px">
    <v-card>
      <v-card-title>Добавление группы</v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        <v-form :data-vv-scope="formId" @submit.prevent="onAdd">
          <v-text-field
            v-if="dialog"
            type="text"
            label="Название группы"
            data-vv-as="Название группы"
            :data-vv-name="`${formId}.name`"
            prepend-icon="fulcrum"
            name="group-name"
            autofocus
            v-model="name"
            v-validate="'required'"
            :error-messages="errors.collect(`${formId}.name`)"
          />
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
import { Groups } from '%/stores/api/SwaggerDocumentationTypescript'
import { GroupEmitEnum } from '%/constants/WindowsEmmit'

@Component
export default class VGroupsAddWindow extends Vue {
  @Inject('$validator') public $validator!: Validator

  dialog: boolean = false
  name: string = ''
  formId: string = 'form-group-add'
  $refs: any
  $options: any

  onReset () {
    Object.assign(this.$data, this.$options.data.call(this))
  }

  mounted () {
    this.$root.$on(GroupEmitEnum.ADD_GROUP, this.onOpenWindow)
  }

  onOpenWindow () {
    this.dialog = true
    setTimeout(() => this.$refs.name.focus(), 100)
  }

  onAdd () {
    this.$validator.validateAll(this.formId)
      .then((res) => {
        if (res) {
          this.$store.state.api.group_Post(new Groups({ name: this.name }))
            .then(() => {
              this.$root.$emit(GroupEmitEnum.CHANGE_GROUP_SUCCESS)
              this.onReset()
            })
            .catch(res => this.$root.$emit('snackbar', res))
        }
      })
  }

  beforeDestroy () {
    this.$root.$off(GroupEmitEnum.ADD_GROUP, this.onOpenWindow)
  }
}

</script>

<style scoped>

</style>
