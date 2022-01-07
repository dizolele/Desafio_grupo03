<template>
  <v-card class="overflow-hidden">
    <v-app-bar
      absolute
      color="#1565C0"
      dark
      shrink-on-scroll
      prominent
      fade-img-on-scroll
      scroll-target="#scrolling-techniques-3"
      height="180px"
    >
      <template v-slot:img="{ props }">
        <v-img v-bind="props"></v-img>
      </template>

      <!-- <v-app-bar-nav-icon></v-app-bar-nav-icon> -->

      <v-app-bar-title class="titulo ml-8">
        <h2>Meetups Iteris</h2>
      </v-app-bar-title>

      <v-btn color= "#FFFF" class="botaologoff">
        <router-link to="/" class="link">
          Logout
          <v-icon>mdi-account</v-icon>
        </router-link>
      </v-btn>
    </v-app-bar>
    <v-sheet
      id="scrolling-techniques-3"
      class="overflow-y-auto"
      max-height="180px"
    >
      <v-container style="height: 1000px"></v-container>
    </v-sheet>
  </v-card>
</template>

<script>
export default {
  name: "AppBar",
};
</script>

<style>
.botaologoff {
  margin-left: 77%;
  font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Oxygen,
    Ubuntu, Cantarell, "Open Sans", "Helvetica Neue", sans-serif;
}

.titulo {
  font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Oxygen,
    Ubuntu, Cantarell, "Open Sans", "Helvetica Neue", sans-serif;
}
.link {
  text-decoration: none;
}
</style>