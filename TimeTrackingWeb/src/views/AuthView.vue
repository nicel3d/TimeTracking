<template>
  <v-auth-component/>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VAuthComponent from '%/components/VAuthComponent.vue'

@Component({
  components: { VAuthComponent }
})
export default class AuthView extends Vue {
}
</script>
