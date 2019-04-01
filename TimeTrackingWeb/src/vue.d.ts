import Vue from 'vue'
import VueRouter from 'vue-router/types'

declare module 'vue/types/vue' {
  // Глобальные свойства можно объявлять
  // на интерфейсе `VueConstructor`

  interface VueConstructor {
    router: VueRouter;
  }
}

declare module 'vue/types/options' {
  interface ComponentOptions<V extends Vue> {
    // this is required because current typings of vee-validate have the old $validates in them, which doesn't work anymore
    $veeValidate?: any;
  }
}
