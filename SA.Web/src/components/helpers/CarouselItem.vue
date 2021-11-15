<template>
    <v-card>
        <v-card-media :src="item.imageUrl" @click="detail(item.id)"></v-card-media>
        <v-layout row wrap>
            <v-flex xs12 class="text-xs-center">
            <h4>{{ item.name }}</h4>
            </v-flex>
        </v-layout>
        <v-layout row wrap class="item-info">
            <v-flex xs4 class="text-xs-center">
            <span>{{ item.info1 }}</span>
            </v-flex>
            <v-flex xs4 class="text-xs-center">
            <span>{{ item.info2 }}</span>
            </v-flex>
            <v-flex xs4 class="text-xs-center">
            <span>{{ item.info3 }}</span>
            </v-flex>
        </v-layout>
        <v-layout row wrap>
            <v-flex xs12 class="text-xs-center">
            <span class="item-price-text">{{ resx('actualPrice') }}</span>
            <span class="price-with-dph" v-if="item.withVat">{{ resx('withVat') }}</span>
            <span class="price-with-dph" v-else>{{ resx('withoutVat') }}</span>
            </v-flex>
        </v-layout>
        <v-layout row wrap>
            <v-flex xs12 class="text-xs-center item-price">
            <h4>
                <price-component :price="item.price" />
            </h4>
            </v-flex>
        </v-layout>
    </v-card>
</template>

<script lang="ts">
import { Component, Prop } from 'vue-property-decorator';

import BaseComponent from '../BaseComponent.vue';
import PriceComponent from './PriceComponent.vue';

import { CarouselItemDto } from './../../poco';

@Component({
  components: {
    PriceComponent,
  },
})
export default class CarouselItem extends BaseComponent {
  @Prop({ default: undefined })
  private item: CarouselItemDto;

  private detail(id: number): void {
      this.$emit('detailCarousel', id);
  }
}
</script>

<style>
h4 {
  font-family: Roboto;
  font-size: 20px;
  font-weight: 500 !important;
  font-style: normal;
  font-stretch: normal;
  line-height: 2.12;
  letter-spacing: 0px;
  color: #000000;
  white-space: normal !important;
}

h4 div {
  line-height: 1.2 !important;
}

.item-info {
  font-family: Roboto;
  font-size: 10px !important;
  font-style: normal;
  font-stretch: normal;
  letter-spacing: 0.8px;
  color: #929292 !important;
}

.item-price-text {
  text-transform: uppercase !important;
  font-family: Roboto;
  font-size: 10px !important;
  font-weight: bold;
  font-style: normal;
  font-stretch: normal;
  letter-spacing: 0.8px;
  color: #030303 !important;
}

.price-with-dph {
  padding-left: 2px;
  text-transform: uppercase;
  font-weight: bold;
  font-size: 10px;
}
</style>