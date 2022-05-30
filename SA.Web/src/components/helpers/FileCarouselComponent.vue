<template>
    <div>
        <v-dialog v-model="isImageSelected" max-width="800">
            <v-img v-if="selectedImagePath" :src="selectedImagePath"></v-img>
        </v-dialog>        
        <div class="fileCarousel" style="position: relative">
            <loading-component :open="isLoading" />
            <v-carousel hide-delimiters :cycle="false">
                <v-carousel-item v-for="(item,i) in record.files"
                                 :key="i"
                                 :src="filePath(item)"
                                 @click="setImage(item)">
                    <v-layout row wrap v-if="isBidding">
                        <v-flex xs12 class="text-xs-right">
                            <v-tooltip top v-if="isWinner">
                                <v-btn icon slot="activator" color="white">
                                    <v-icon small color="green" style="cursor: pointer">thumb_up</v-icon>
                                </v-btn>
                                <span>{{ resx('winning') }} </span>
                            </v-tooltip>
                            <v-tooltip top v-else>
                                <v-btn icon slot="activator" color="white">
                                    <v-icon small color="red" style="cursor: pointer">thumb_down</v-icon>
                                </v-btn>
                                <span>{{ resx('notWinning') }} </span>
                            </v-tooltip>
                        </v-flex>
                    </v-layout>
                </v-carousel-item>
            </v-carousel>
        </div>
    </div>
</template>

<script lang="ts">

    import { Component, Prop } from 'vue-property-decorator';
    import { Getter, Action, namespace } from 'vuex-class';

    import BaseComponent from '../BaseComponent.vue';
    import LoadingComponent from './LoadingComponent.vue';

    import { Record } from '../../model';

    import { FileSimpleDto } from '../../poco';

    const RecordGetter = namespace('record', Getter);
    const RecordAction = namespace('record', Action);

    @Component({
        components: {
            LoadingComponent,
        },
    })
    export default class FileCarouselComponent extends BaseComponent {
        @Prop({ default: -1 })
        private recordId: string;

        @Prop({ default: false })
        private isWinner: boolean;

        @Prop({ default: false })
        private isBidding: boolean;

        @RecordGetter('getCurrent')
        private record: Record;

        @RecordAction('getRecordFiles')
        private files: any;

        private isLoading: boolean = false;

        private selectedImagePath: string = null;
        private isImageSelected: boolean = false;

        private mounted() {
            const id: number = parseInt(this.recordId, 10);

            if (id > -1) {
                this.isLoading = true;

                this.files(id).then((result) => {
                    this.isLoading = false;
                });
            }
        }

        private filePath(file: FileSimpleDto): string {
            return `${this.settings.apiUrl.replace('/api', '')}/${file.path}/${file.recordId}/images/${file.name}`;
        }

        private setImage(file: FileSimpleDto): void {
            this.selectedImagePath = this.filePath(file);
            this.isImageSelected = true;
        }
    }

</script>

<style>
    .v-dialog__content {
        position: absolute !important;
    }

    .fileCarousel .v-btn .v-btn__content .v-icon {
        color: #e0e0e0;
    }

    .fileCarousel .v-btn .v-btn__content .v-icon:hover {
        color: #858585;
    }
</style>