import Vue from 'vue'

declare module 'vue/types/vue' {
  // Глобальные свойства можно объявлять
  // на интерфейсе `VueConstructor`
  interface VueConstructor {
  }
}

declare module 'vue/types/options' {
  interface ComponentOptions<V extends Vue> {
    // this is required because current typings of vee-validate have the old $validates in them, which doesn't work anymore
    $veeValidate?: any;
  }
}
