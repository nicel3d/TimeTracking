<template>
  <v-dialog-full-window
    @on-save="onSave"
    :item="item"
    :loading="loading"
    v-model="dialog">
    <span slot="title">Детали активности</span>
    <template v-if="item">
      <v-container grid-list-xl>
        <v-layout wrap justify-space-between>
          <v-flex xs12>
            <div class="mb-4">
              <vue-picture-swipe class="text-xs-center image-origin" :items="[image]"></vue-picture-swipe>
            </div>
          </v-flex>

          <v-flex xs12 md6>
            <v-text-field
              :value="item.application.caption"
              label="Название программы"
              readonly
            ></v-text-field>
          </v-flex>
          <v-flex xs12 md6>
            <v-text-field
              :value="item.applicationTitle"
              label="Заголовок программы"
              readonly
            ></v-text-field>
          </v-flex>

          <v-flex xs12 md6>
            <v-text-field
              :value="item.staff.caption"
              label="Пользователь"
              readonly
            ></v-text-field>
          </v-flex>

          <v-flex xs12 md6>
            <v-text-field
              :value="item.updatedAt.toLocaleDateString()"
              label="Дата"
              readonly
            ></v-text-field>
          </v-flex>

          <v-flex xs12 md6>
            <v-text-field
              :value="item.updatedAt.toLocaleDateString('ru', {weekday: 'long'})"
              label="День недели"
              readonly
            ></v-text-field>
          </v-flex>

          <v-flex xs12 sm6>
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
        </v-layout>
      </v-container>
      <v-divider></v-divider>
    </template>
  </v-dialog-full-window>
</template>

<script lang="ts">
import { Component } from 'vue-property-decorator'
import SkipTake from '%/utils/SkipTake'
import { ActivityStaff, StateEnum } from '%/stores/api/SwaggerDocumentationTypescript'
import { oc } from 'ts-optchain'
import { States } from '%/constants/States'
import VDialogFullWindow from '%/utils/VDialogFullWindow.vue'

@Component({
  components: { VDialogFullWindow }
})
export default class VWindowEditActivityStaff extends SkipTake {
  item: ActivityStaff | null = null
  dialog: boolean = false
  loading: boolean = true
  state: StateEnum = StateEnum.Neutral
  states = States

  get image () {
    return {
      src: 'data:image/jpeg;base64,' + oc(this.item).imageOrigin(''),
      thumbnail: 'data:image/jpeg;base64,' + oc(this.item).imageOrigin(''),
      w: 1920,
      h: 1080,
      title: oc(this.item).application.caption('')
    }
  }

  onView (id) {
    this.dialog = true
    this.loading = true
    this.$store.state.api.activityStaff_Get(id)
      .then(res => {
        this.item = res
        this.state = res.application.state
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  onSave () {
    if (!this.item || !this.item.applicationId) return

    this.loading = true
    this.$store.state.api.applications_PutState(this.item.applicationId, this.state)
      .then(() => {
        this.$emit('on-success')
        this.item = null
        this.dialog = false
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }
}
</script>

<style>
.image-origin [itemprop="thumbnail"] {
  max-width: 100%;
  max-height: 450px;
}
</style>
