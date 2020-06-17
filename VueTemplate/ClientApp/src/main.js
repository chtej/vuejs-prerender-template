import { sync } from 'vuex-router-sync';
import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';

Vue.config.productionTip = false;

export const createApp = (context) => {
	// sync so that route state is available as part of the store
	sync(store, router);

	const app = new Vue({
		data: { url: context ? context.url : '' },
		router,
		store,
		render: (h) => h(App)
	});

	return { app, router, store };
};
