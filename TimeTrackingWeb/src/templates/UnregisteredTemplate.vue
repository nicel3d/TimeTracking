<template>
  <v-app id="inspire">
    <v-content>
      <v-container
        fluid
        class="blue-grey lighten-5"
        fill-height>
        <v-layout
          align-center
          justify-center>
          <router-view/>
        </v-layout>
      </v-container>
    </v-content>
  </v-app>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component
export default class UnregisteredTemplate extends Vue {
}
</script>
