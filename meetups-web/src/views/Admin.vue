<template>
<div>
    <lista-eventos :tipoUser="'admin'"/>
</div>
  
</template>

<script>
import ListaEventos from './ListaEventos.vue'
export default {
  components: { ListaEventos },
    name: "Admin",
    componentes: {
        ListaEventos
    }
}
</script>

<style>

</style>