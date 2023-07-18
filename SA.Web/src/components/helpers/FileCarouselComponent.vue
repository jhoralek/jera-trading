<template>
    <div>
        <v-dialog v-model="isImageSelected" max-width="1000">
            <v-layout row wrap>
                <lingallery
                    :iid.sync="currentId"
                    :responsive="true"
                    :items="gallery"
                    baseColor="#999"></lingallery>
            </v-layout>
        </v-dialog>
        <div class="fileCarousel" style="position: relative">
            <loading-component :open="isLoading" />
            <v-img v-if="gallery" :src="firstImage()" max-height="400" @click="openDialog()">
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
            </v-img>
        </div>
    </div>
</template>

<script lang="ts">

    import { Component, Prop } from 'vue-property-decorator';
    import { Getter, Action, namespace } from 'vuex-class';

    import BaseComponent from '../BaseComponent.vue';
    import LoadingComponent from './LoadingComponent.vue';
    import Linggallery from 'lingallery';


    import { Record } from '../../model';

    import { FileSimpleDto } from '../../poco';

    const RecordGetter = namespace('record', Getter);
    const RecordAction = namespace('record', Action);

    @Component({
        components: {
            LoadingComponent,
            Linggallery,
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

        private isImageSelected: boolean = false;
        private gallery = [];
        private currentId = null;

        private mounted() {
            const id: number = parseInt(this.recordId, 10);

            if (id > -1) {
                this.isLoading = true;

                this.files(id).then((result) => {
                    this.isLoading = false;
                    this.composeGalleryImages();
                });
            }
        }

        private filePath(file: FileSimpleDto): string {
            return `${this.settings.apiUrl.replace('/api', '')}/${file.path}/${file.recordId}/images/${file.name}`;
        }

        private openDialog(): void {
            this.isImageSelected = true;
        }

        private firstImage(): string {
            return this.gallery[0].src;
        }

        private composeGalleryImages(): void {
            this.gallery = this.record.files.map((item) => {
                return {
                    src: this.filePath(item),
                    thumbnail: this.filePath(item),
                    id: item.id,
                };
            });
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