<template>
<div>
    <lista-eventos :tipoUser="'user'"/>
</div>
  
</template>

<script>
import ListaEventos from './ListaEventos.vue'
export default {
  components: { ListaEventos },
    name: "User",
    componentes: {
        ListaEventos
    }
}
</script>

<style>

</style>