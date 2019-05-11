<template>
  <v-dialog v-model="dialog" max-width="300px">
    <v-card>
      <v-card-title>Добавление группу</v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        <v-form @submit.prevent="onAdd">
          <v-text-field
            v-validate="'required'"
            ref="name"
            v-model="name"
            :error-messages="errors.collect('group-name')"
            data-vv-name="group-name"
            prepend-icon="fulcrum"
            name="group-name"
            label="Название группы"
            type="text"
            required
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
import { GroupEmitEnum } from '%/constants/windows/GroupsWindows'

@Component
export default class VGroupsAddWindows extends Vue {
  @Inject('$validator') public $validator!: Validator

  dialog: boolean = false
  name: string = ''
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
    this.$validator.validateAll()
      .then((res) => {
        if (res) {
          this.$store.state.api.group_Post(new Groups({ name: this.name }))
            .then(() => {
              this.$root.$emit(GroupEmitEnum.ADD_GROUP_SUCCESS)
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
